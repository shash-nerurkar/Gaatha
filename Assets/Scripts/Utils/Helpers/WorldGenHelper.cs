using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using Helpers;
using System.Text;
using System.Collections.Generic;

namespace WorldGeneration
{
    public static class WorldGenHelper
    {
        public static float GetTilemapCellRadius ( Tilemap outputTilemap ) {
            return Mathf.Sqrt ( outputTilemap.cellSize.x * outputTilemap.cellSize.x + outputTilemap.cellSize.y * outputTilemap.cellSize.y ) / 2;
        }



        public static Vector3Int [ ] GenerateShapeVertexPoints ( ElementShape shape, Vector3Int position, Vector3Int dimensions ) {
            switch ( shape ) {
                default:
                case ElementShape.Quad:
                    return WorldGenHelper.GenerateQuadVertexPoints ( position: position, dimensions: dimensions );
                    
                case ElementShape.Circle:
                    return WorldGenHelper.GenerateCircleVertexPoints ( position: position, dimensions: dimensions );
                    
                case ElementShape.Hexagon:
                    return WorldGenHelper.GenerateHexagonVertexPoints ( position: position, dimensions: dimensions );
                    
                case ElementShape.RegularPolygon:
                    return WorldGenHelper.GenerateRandomRegularPolygonVertexPoints ( position: position, dimensions: dimensions );
                    
                case ElementShape.RandomPolygon:
                    return WorldGenHelper.GenerateRandomPolygonVertexPoints ( position: position, dimensions: dimensions );
            }
        }

        private static Vector3Int [ ] GenerateQuadVertexPoints ( Vector3Int position, Vector3Int dimensions ) {
            return new Vector3Int [ 4 ] {
                position + new Vector3Int ( 0,                0                   ) - dimensions / 2,
                position + new Vector3Int ( 0,                dimensions.y        ) - dimensions / 2,
                position + new Vector3Int ( dimensions.x,     dimensions.y        ) - dimensions / 2,
                position + new Vector3Int ( dimensions.x,     0                   ) - dimensions / 2
            };
        }

        private static Vector3Int [ ] GenerateCircleVertexPoints ( Vector3Int position, Vector3Int dimensions ) {
            int vertexCount = 90;
            
            Vector3Int [ ] randomPolygonVertexPoints = new Vector3Int [ vertexCount ];

            float angleIncrement = 360f / vertexCount;
            for ( int i = 0; i < vertexCount; i++ ) {
                float angle = i * angleIncrement * Mathf.Deg2Rad;
                
                float x = Mathf.Cos ( angle ) * dimensions.x / 2;
                float y = Mathf.Sin ( angle ) * dimensions.y / 2;

                randomPolygonVertexPoints [ i ] = position + new Vector3Int ( Mathf.RoundToInt ( x ), Mathf.RoundToInt ( y ), 0 );
            }

            return randomPolygonVertexPoints;
        }

        private static Vector3Int [ ] GenerateHexagonVertexPoints ( Vector3Int position, Vector3Int dimensions ) {
            return new Vector3Int [ 6 ] {
                position + new Vector3Int ( 0,                            dimensions.y * 2 / 3    ) - dimensions / 2,
                position + new Vector3Int ( dimensions.x / 3,             dimensions.y            ) - dimensions / 2,
                position + new Vector3Int ( dimensions.x * 2 / 3,         dimensions.y            ) - dimensions / 2,
                position + new Vector3Int ( dimensions.x,                 dimensions.y / 3        ) - dimensions / 2,
                position + new Vector3Int ( dimensions.x * 2 / 3,         0                       ) - dimensions / 2,
                position + new Vector3Int ( dimensions.x / 3,             0                       ) - dimensions / 2
            };
        }

        private static Vector3Int [ ] GenerateRandomRegularPolygonVertexPoints ( Vector3Int position, Vector3Int dimensions ) {
            int vertexCount = Random.Range ( 5, 9 );
            
            Vector3Int [ ] vertexPoints = new Vector3Int [ vertexCount ];

            for ( int i = 0; i < vertexCount; i++ ) {
                float angle = i * ( 360f / vertexCount ) * Mathf.Deg2Rad;
                
                float x = Mathf.Cos ( angle ) * dimensions.x / 2;
                float y = Mathf.Sin ( angle ) * dimensions.y / 2;

                vertexPoints [ i ] = position + new Vector3Int ( Mathf.RoundToInt ( x ), Mathf.RoundToInt ( y ), 0 );
            }

            return vertexPoints;
        }

        private static Vector3Int [ ] GenerateRandomPolygonVertexPoints ( Vector3Int position, Vector3Int dimensions ) {
            int vertexCount = Random.Range ( 9, 18 );
            
            List < Vector3Int > vertexPoints = new List < Vector3Int >( );
            float angle = 0;

            for ( int i = 0; i < vertexCount; i++ ) {
                angle += Random.Range ( 0, 360f * 2 / vertexCount ) * Mathf.Deg2Rad;
                if ( angle > 360f * Mathf.Deg2Rad )
                    break;
                
                float vertexCenterDistMultiplier = Random.Range ( 0.1f, 0.5f );

                float x = Mathf.Cos ( angle ) * dimensions.x * vertexCenterDistMultiplier;
                float y = Mathf.Sin ( angle ) * dimensions.y * vertexCenterDistMultiplier;

                vertexPoints.Add ( position + new Vector3Int ( Mathf.RoundToInt ( x ), Mathf.RoundToInt ( y ), 0 ) );
            }

            return vertexPoints.ToArray ( );
        }



        public static bool IsPointInsideShape ( Vector3Int point, Vector3Int[ ] shapeVertexPoints ) {
            int IsLeft ( Vector2Int a, Vector2Int b, Vector2Int c ) =>  ( ( b.x - a.x ) * ( c.y - a.y ) - ( c.x - a.x ) * ( b.y - a.y ) );

            int windingNumber = 0;
            int vertexCount = shapeVertexPoints.Length;

            for ( int i = 0; i < vertexCount; i++ ) {
                Vector2Int currentVertex = ( Vector2Int ) shapeVertexPoints [ i ];
                Vector2Int nextVertex = ( Vector2Int ) shapeVertexPoints [ ( i + 1 ) % vertexCount ];

                if ( currentVertex.y  <= point.y ) {
                    if ( nextVertex.y > point.y && IsLeft( currentVertex, nextVertex, ( Vector2Int ) point ) > 0 )
                        windingNumber++;
                }
                else {
                    if ( nextVertex.y <= point.y && IsLeft( currentVertex, nextVertex, ( Vector2Int ) point ) < 0 )
                        windingNumber--;
                }
            }

            return windingNumber != 0;
        }
        
        public static bool IsShapeInsideShape ( Vector3Int[ ] innerShapeVertexPoints, Vector3Int[ ] outerShapeVertexPoints ) {
            foreach ( Vector3Int vertexPoint in innerShapeVertexPoints ) {
                if ( !IsPointInsideShape ( point: vertexPoint, shapeVertexPoints: outerShapeVertexPoints ) )
                    return false;
            }

            return true;
        }
        
        public static bool IsShapeIntersectingShape ( Vector3Int[ ] firstShapeVertexPoints, Vector3Int[ ] secondShapeVertexPoints, Vector3Int firstShapeCenterPoint ) {
            foreach ( Vector3Int vertexPoint in firstShapeVertexPoints ) {
                if ( IsPointInsideShape ( point: vertexPoint, shapeVertexPoints: secondShapeVertexPoints ) )
                    return true;
            }

            if ( IsPointInsideShape ( point: firstShapeCenterPoint, shapeVertexPoints: secondShapeVertexPoints ) )
                return true;

            return false;
        }
        
        public static Vector3Int GetRandomPointInsideShape ( Vector3Int[ ] vertices, Tilemap outputTilemap ) {
            float minXCoord = float.MaxValue;
            float maxXCoord = float.MinValue;
            float minYCoord = float.MaxValue;
            float maxYCoord = float.MinValue;

            foreach ( Vector3Int vertex in vertices ) {
                Vector3 vertexCoord = outputTilemap.CellToWorld ( vertex );

                minXCoord = Mathf.Min ( minXCoord, vertexCoord.x );
                maxXCoord = Mathf.Max ( maxXCoord, vertexCoord.x );
                minYCoord = Mathf.Min ( minYCoord, vertexCoord.y );
                maxYCoord = Mathf.Max ( maxYCoord, vertexCoord.y );
            }

            Vector3Int randomPoint;
            do {
                randomPoint = outputTilemap.WorldToCell ( new Vector3 ( Random.Range ( minXCoord, maxXCoord + 1 ), Random.Range ( minYCoord, maxYCoord + 1 )  ) );
            } while ( !IsPointInsideShape ( point: randomPoint, shapeVertexPoints: vertices ) );

            return randomPoint;
        }
    

    
        public static Vector3Int[] GetPointsBetween ( Vector3Int start, Vector3Int end ) {
            int minX = Mathf.Min ( start.x, end.x );
            int minY = Mathf.Min ( start.y, end.y );
            int minZ = Mathf.Min ( start.z, end.z );

            int maxX = Mathf.Max ( start.x, end.x );
            int maxY = Mathf.Max ( start.y, end.y );
            int maxZ = Mathf.Max ( start.z, end.z );

            Vector3Int [ ] points = new Vector3Int [ ( maxX - minX + 1 ) * ( maxY - minY + 1 ) * ( maxZ - minZ + 1 ) ];

            int index = 0;

            for ( int x = minX; x <= maxX; x++ )
                for ( int y = minY; y <= maxY; y++ )
                    for ( int z = minZ; z <= maxZ; z++ ) {
                        points[ index ] = new Vector3Int ( x, y, z );
                        index++;
                    }

            return points;
        }
    }


    [Serializable]
    public enum ElementShape {
        Quad,
        Circle,
        Hexagon,
        RegularPolygon,
        RandomPolygon
    }
}