using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaxWorldGeneration
{
    public class WaxRoom : MonoBehaviour
    {
        private ShapeRenderer shapeRenderer;
        private Tilemap OutputTilemap;
        private TileBase DefaultTile;

        public ElementShape RoomShape { get; private set; }
        public int RoomElevation { get; private set; }
        public Vector3Int RoomPosition { get; private set; }
        public Vector3Int RoomDimensions { get; private set; }

        private Vector3Int[] roomVertexPoints;

        void Awake () => WaxWorldGenMain.ClearFloorAction += DestroyRoom;
    
        private void DestroyRoom () => Destroy ( gameObject );

        void OnDestroy () => WaxWorldGenMain.ClearFloorAction -= DestroyRoom;

        public void InitRoom ( Tilemap outputTilemap, TileBase defaultTile, ElementShape roomShape, int roomElevation, Vector3Int roomDimensions ) {
            this.OutputTilemap = outputTilemap;
            this.DefaultTile = defaultTile;

            this.RoomShape = roomShape;
            this.RoomPosition = OutputTilemap.WorldToCell ( transform.position );
            this.RoomDimensions = roomDimensions;
            this.RoomElevation = roomElevation;
            
            GenerateRoomVertices ();
            CreateRoom ();
        }

        public void GenerateRoomVertices (  ) {
            Vector3Int[] GenerateQuadPoints() {
                return new Vector3Int[4] {
                    RoomPosition + new Vector3Int( 0,                   0                   ) - RoomDimensions / 2,
                    RoomPosition + new Vector3Int( 0,                   RoomDimensions.y    ) - RoomDimensions / 2,
                    RoomPosition + new Vector3Int( RoomDimensions.x,    RoomDimensions.y    ) - RoomDimensions / 2,
                    RoomPosition + new Vector3Int( RoomDimensions.x,    0                   ) - RoomDimensions / 2
                };
            }

            Vector3Int[] GenerateCirclePoints() {
                int vertexCount = 50;
                
                Vector3Int[] randomPolygonVertexPoints = new Vector3Int [ vertexCount ];

                float angleIncrement = 360f / vertexCount;
                for ( int i = 0; i < vertexCount; i++ ) {
                    float angle = i * angleIncrement * Mathf.Deg2Rad;
                    
                    float x = Mathf.Cos ( angle ) * RoomDimensions.x / 2;
                    float y = Mathf.Sin ( angle ) * RoomDimensions.y / 2;

                    randomPolygonVertexPoints[ i ] = RoomPosition + new Vector3Int ( Mathf.RoundToInt ( x ), Mathf.RoundToInt ( y ), 0 );
                }

                return randomPolygonVertexPoints;
            }

            Vector3Int[] GenerateHexagonPoints() {
                return new Vector3Int[6] {
                    RoomPosition + new Vector3Int( 0,                           RoomDimensions.y * 2 / 3    ) - RoomDimensions / 2,
                    RoomPosition + new Vector3Int( RoomDimensions.x / 3,        RoomDimensions.y            ) - RoomDimensions / 2,
                    RoomPosition + new Vector3Int( RoomDimensions.x * 2 / 3,    RoomDimensions.y            ) - RoomDimensions / 2,
                    RoomPosition + new Vector3Int( RoomDimensions.x,            RoomDimensions.y / 3        ) - RoomDimensions / 2,
                    RoomPosition + new Vector3Int( RoomDimensions.x * 2 / 3,    0                           ) - RoomDimensions / 2,
                    RoomPosition + new Vector3Int( RoomDimensions.x / 3,        0                           ) - RoomDimensions / 2
                };
            }

            Vector3Int[] GenerateRandomPolygonPoints() {
                int vertexCount = Random.Range ( 3, 12 );
                
                Vector3Int[] randomPolygonVertexPoints = new Vector3Int [ vertexCount ];

                float angleIncrement = 360f / vertexCount;
                for ( int i = 0; i < vertexCount; i++ ) {
                    float angle = i * angleIncrement * Mathf.Deg2Rad;
                    
                    float x = Mathf.Cos( angle ) * RoomDimensions.x / 2;
                    float y = Mathf.Sin( angle ) * RoomDimensions.y / 2;

                    randomPolygonVertexPoints[ i ] = RoomPosition + new Vector3Int ( Mathf.RoundToInt ( x ), Mathf.RoundToInt ( y ), 0 );
                }

                return randomPolygonVertexPoints;
            }
            
            switch( RoomShape ) {
                default:
                case ElementShape.Quad:
                    roomVertexPoints = GenerateQuadPoints();
                    break;
                    
                case ElementShape.Circle:
                    roomVertexPoints = GenerateCirclePoints();
                    break;
                    
                case ElementShape.Hexagon:
                    roomVertexPoints = GenerateHexagonPoints();
                    break;
                    
                case ElementShape.Polygon:
                    roomVertexPoints = GenerateRandomPolygonPoints();
                    break;  
            }
        }

        private void CreateRoom () {
            DrawRoomGizmo();

            TileRoom();
        }

        private void TileRoom () {
            bool IsPointInsidePolygon( Vector3Int point, Vector3Int[] polygon ) {
                int IsLeft ( Vector3Int a, Vector3Int b, Vector3Int c ) => ( ( b.x - a.x ) * ( c.y - a.y ) - ( c.x - a.x ) * ( b.y - a.y ) );

                int windingNumber = 0;
                int vertexCount = polygon.Length;

                for ( int i = 0; i < vertexCount; i++ ) {
                    Vector3Int currentVertex = polygon[ i ];
                    Vector3Int nextVertex = polygon[ ( i + 1 ) % vertexCount ];

                    if ( currentVertex.y <= point.y ) {
                        if ( nextVertex.y > point.y && IsLeft( currentVertex, nextVertex, point ) > 0 )
                            windingNumber++;
                    }
                    else {
                        if ( nextVertex.y <= point.y && IsLeft( currentVertex, nextVertex, point ) < 0 )
                            windingNumber--;
                    }
                }

                return windingNumber != 0;
            }

            List<Vector3Int> pointsToFill = new List<Vector3Int>();

            for ( int i = 0; i < RoomDimensions.x; i++ ) {
                for ( int j = 0; j < RoomDimensions.y; j++ ) {
                    Vector3Int currentPoint = RoomPosition + new Vector3Int ( i, j ) - RoomDimensions / 2;

                    if ( IsPointInsidePolygon ( currentPoint, roomVertexPoints ) )
                        pointsToFill.Add ( currentPoint );
                }
            }

            foreach ( Vector3Int point in pointsToFill )
                OutputTilemap.SetTile ( point, DefaultTile );
        }

        private void DrawRoomGizmo () {
            shapeRenderer = GetComponent<ShapeRenderer>();

            if( shapeRenderer != null ) {
                Vector3[] roomVertexCoords = new Vector3[ roomVertexPoints.Length ];
                for( int i = 0; i < roomVertexPoints.Length; i++ )
                    roomVertexCoords[i] = OutputTilemap.CellToWorld( roomVertexPoints[ i ] );

                shapeRenderer.DrawPolygon( roomVertexCoords, Color.blue, 0.1f );
            }
        }
}
}

