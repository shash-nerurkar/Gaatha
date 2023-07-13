using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaxWorldGeneration
{
    public class WaxRoom : MonoBehaviour
    {
        private ShapeRenderer shapeRenderer;
        private Tilemap outputTilemap;
        private WaxTilePlacer tilePlacer;

        public ElementShape RoomShape { get; private set; }
        public Vector3Int RoomPosition { get; private set; }
        public Vector3Int RoomDimensions { get; private set; }

        public Vector3Int[] RoomVertexPoints { get; private set; }

        private List<Vector3Int> roomFloorPoints = new List<Vector3Int>();
 
        public void DestroyRoom () {
            foreach ( Vector3Int point in roomFloorPoints )
                outputTilemap.SetTile ( point, null );

            Destroy ( gameObject );
        }

        public void InitRoom ( Tilemap outputTilemap, WaxTilePlacer tilePlacer, ElementShape roomShape, Vector3Int roomDimensions ) {
            this.outputTilemap = outputTilemap;
            this.tilePlacer = tilePlacer;

            this.RoomShape = roomShape;
            this.RoomPosition = outputTilemap.WorldToCell ( transform.position );
            this.RoomDimensions = roomDimensions;

            GenerateRoomVertexPoints ( roomCoords: transform.position );
        }

        public void GenerateRoomVertexPoints ( Vector3 roomCoords ) {
            transform.position = roomCoords;
            this.RoomPosition = outputTilemap.WorldToCell ( transform.position );

            RoomVertexPoints = new Vector3Int [ 0 ];
            roomFloorPoints.Clear ();

            RoomVertexPoints = WorldGenHelper.GenerateShapeVertexPoints ( shape: RoomShape, centerPoint: RoomPosition, dimensions: RoomDimensions );
        
            DrawRoomGizmo ();
        }

        public void TileRoom () {
            for ( int i = 0; i < RoomDimensions.x; i++ ) {
                for ( int j = 0; j < RoomDimensions.y; j++ ) {
                    Vector3Int currentPoint = RoomPosition + new Vector3Int ( i, j ) - RoomDimensions / 2;

                    if ( WorldGenHelper.IsPointInsideShape ( point: currentPoint, shapeVertexPoints: RoomVertexPoints ) )
                        roomFloorPoints.Add ( currentPoint );
                }
            }

            tilePlacer?.PlaceFloorTiles ( points: roomFloorPoints.ToArray (), outputTilemap: outputTilemap );
        }

        private void DrawRoomGizmo () {
            shapeRenderer = GetComponent<ShapeRenderer>();

            if( shapeRenderer != null ) {
                Vector3[] roomVertexCoords = new Vector3[ RoomVertexPoints.Length ];
                for( int i = 0; i < RoomVertexPoints.Length; i++ )
                    roomVertexCoords[i] = outputTilemap.CellToWorld( RoomVertexPoints[ i ] );

                shapeRenderer.DrawPolygon( roomVertexCoords, Color.blue, 0.1f );
            }
        }
    }
}

