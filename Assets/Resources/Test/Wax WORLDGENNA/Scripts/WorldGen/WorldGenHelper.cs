using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using System.Text;

namespace WorldGeneration
{
    public static class WorldGenHelper
    {
        public static Vector3Int GetRandomVectorBetween ( Vector3Int minVector, Vector3Int maxVector ) {
            return new Vector3Int ( 
                Random.Range ( minVector.x, maxVector.x ),
                Random.Range ( minVector.y, maxVector.y ),
                Random.Range ( minVector.z, maxVector.z )
            );
        }

        public static Vector3Int Vector3IntLerp ( Vector3Int a, Vector3Int b, float t ) {
            int x = Mathf.RoundToInt ( Mathf.Lerp ( a.x, b.x, t ) );
            int y = Mathf.RoundToInt ( Mathf.Lerp ( a.y, b.y, t ) );
            int z = Mathf.RoundToInt ( Mathf.Lerp ( a.z, b.z, t ) );

            return new Vector3Int(x, y, z);
        }



        public static void SortVector3IntArray ( Vector3Int[ ] array ) {
            Array.Sort ( array, new Vector3IntComparer ( ) );
        }
    
        public static bool ArrayContains ( Vector3Int[ ] array, Vector3Int target ) {
            if ( array == null || array.Length == 0 )
                return false;

            for ( int i = 0; i < array.Length; i++ )
                if ( target.Equals ( array[ i ] ) )
                    return true;

            return false;
        }
    
        public static String ArrayToString ( Vector3Int[ ] array ) {
            StringBuilder s = new StringBuilder ( "{" );

            foreach ( Vector3Int vector in array )
                s.Append ( "new Vector3Int " + vector.ToString ( ) + ", " );

            s.Append( "}" );

            return s.ToString ( );
        } 



        public static float GetTilemapCellRadius ( Tilemap outputTilemap ) {
            return Mathf.Sqrt ( outputTilemap.cellSize.x * outputTilemap.cellSize.x + outputTilemap.cellSize.y * outputTilemap.cellSize.y ) / 2;
        }



        public static Vector3Int [ ] GenerateShapeVertexPoints ( ElementShape shape, Vector3Int centerPoint, Vector3Int dimensions ) {
            switch ( shape ) {
                default:
                case ElementShape.Quad:
                    return WorldGenHelper.GenerateQuadVertexPoints ( centerPoint: centerPoint, dimensions: dimensions );
                    
                case ElementShape.Circle:
                    return WorldGenHelper.GenerateCircleVertexPoints ( centerPoint: centerPoint, dimensions: dimensions );
                    
                case ElementShape.Hexagon:
                    return WorldGenHelper.GenerateHexagonVertexPoints ( centerPoint: centerPoint, dimensions: dimensions );
                    
                case ElementShape.Polygon:
                    return WorldGenHelper.GenerateRandomPolygonVertexPoints ( centerPoint: centerPoint, dimensions: dimensions );
            }
        }

        private static Vector3Int [ ] GenerateQuadVertexPoints ( Vector3Int centerPoint, Vector3Int dimensions ) {
            return new Vector3Int [ 4 ] {
                centerPoint + new Vector3Int ( 0,                0                   ) - dimensions / 2,
                centerPoint + new Vector3Int ( 0,                dimensions.y        ) - dimensions / 2,
                centerPoint + new Vector3Int ( dimensions.x,     dimensions.y        ) - dimensions / 2,
                centerPoint + new Vector3Int ( dimensions.x,     0                   ) - dimensions / 2
            };
        }

        private static Vector3Int [ ] GenerateCircleVertexPoints ( Vector3Int centerPoint, Vector3Int dimensions ) {
            int vertexCount = 50;
            
            Vector3Int [ ] randomPolygonVertexPoints = new Vector3Int [ vertexCount ];

            float angleIncrement = 360f / vertexCount;
            for ( int i = 0; i < vertexCount; i++ ) {
                float angle = i * angleIncrement * Mathf.Deg2Rad;
                
                float x = Mathf.Cos ( angle ) * dimensions.x / 2;
                float y = Mathf.Sin ( angle ) * dimensions.y / 2;

                randomPolygonVertexPoints [ i ] = centerPoint + new Vector3Int ( Mathf.RoundToInt ( x ), Mathf.RoundToInt ( y ), 0 );
            }

            return randomPolygonVertexPoints;
        }

        private static Vector3Int [ ] GenerateHexagonVertexPoints ( Vector3Int centerPoint, Vector3Int dimensions ) {
            return new Vector3Int [ 6 ] {
                centerPoint + new Vector3Int ( 0,                            dimensions.y * 2 / 3    ) - dimensions / 2,
                centerPoint + new Vector3Int ( dimensions.x / 3,             dimensions.y            ) - dimensions / 2,
                centerPoint + new Vector3Int ( dimensions.x * 2 / 3,         dimensions.y            ) - dimensions / 2,
                centerPoint + new Vector3Int ( dimensions.x,                 dimensions.y / 3        ) - dimensions / 2,
                centerPoint + new Vector3Int ( dimensions.x * 2 / 3,         0                       ) - dimensions / 2,
                centerPoint + new Vector3Int ( dimensions.x / 3,             0                       ) - dimensions / 2
            };
        }

        private static Vector3Int [ ] GenerateRandomPolygonVertexPoints ( Vector3Int centerPoint, Vector3Int dimensions ) {
            int vertexCount = Random.Range ( 6, 9 );
            
            Vector3Int [ ] randomPolygonVertexPoints = new Vector3Int [ vertexCount ];

            float angleIncrement = 360f / vertexCount;
            for ( int i = 0; i < vertexCount; i++ ) {
                float angle = i * angleIncrement * Mathf.Deg2Rad;
                
                float x = Mathf.Cos ( angle ) * dimensions.x / 2;
                float y = Mathf.Sin ( angle ) * dimensions.y / 2;

                randomPolygonVertexPoints [ i ] = centerPoint + new Vector3Int ( Mathf.RoundToInt ( x ), Mathf.RoundToInt ( y ), 0 );
            }

            return randomPolygonVertexPoints;
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



        public static Vector3Int GetMinPositionValues ( Vector3Int[ ] vectorArray ) {
            Vector3Int minVector = new Vector3Int ( );

            if ( vectorArray != null && vectorArray.Length != 0 ) {
                minVector = vectorArray [ 0 ];

                for ( int index = 1; index < vectorArray.Length; index++ ) {
                    if ( vectorArray [ index ].x  <  minVector.x )
                        minVector.x = vectorArray [ index ].x;

                    if ( vectorArray [ index ].y  <  minVector.y )
                        minVector.y = vectorArray [ index ].y;

                    if ( vectorArray [ index ].z  <  minVector.z )
                        minVector.z = vectorArray [ index ].z;
                }
            }

            return minVector;
        }

        public static Vector3Int GetMaxPositionValues ( Vector3Int[ ] vectorArray ) {
            Vector3Int maxVector = new Vector3Int ( );

            if ( vectorArray != null && vectorArray.Length != 0 ) {
                maxVector = vectorArray [ 0 ];

                for ( int index = 1; index < vectorArray.Length; index++ ) {
                    if ( vectorArray [ index ].x > maxVector.x )
                        maxVector.x = vectorArray [ index ].x;

                    if ( vectorArray [ index ].y > maxVector.y )
                        maxVector.y = vectorArray [ index ].y;

                    if ( vectorArray [ index ].z > maxVector.z )
                        maxVector.z = vectorArray [ index ].z;
                }
            }

            return maxVector;
        }
    }


    public class Vector3IntComparer : IComparer < Vector3Int > 
    {
        public int Compare( Vector3Int a, Vector3Int b ) {
            if (a.x != b.x)
                return a.x.CompareTo(b.x);

            if (a.y != b.y)
                return a.y.CompareTo(b.y);

            if (a.z != b.z)
                return a.z.CompareTo(b.z);

            return 0;
        }
    }


    [Serializable]
    public enum ElementShape {
        Quad,
        Circle,
        Hexagon,
        Polygon
    }
}