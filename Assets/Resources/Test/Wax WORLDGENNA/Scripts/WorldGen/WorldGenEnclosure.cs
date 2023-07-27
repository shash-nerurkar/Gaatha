using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace WorldGeneration
{
    public abstract class WorldGenEnclosure : MonoBehaviour
    {
        public ShapeRenderer ShapeRenderer { get; private set; }
        public Tilemap OutputTilemap { get; private set; }
        public WorldGenTilePlacer TilePlacer { get; private set; }

        
        public ElementShape Shape { get; private set; }
        public Vector3Int Position {
            get { return OutputTilemap.WorldToCell ( transform.position ); }
        }
        public Vector3Int Dimensions { get; private set; }
        public float Radius {
            get { 
                return Mathf.Max ( ( float ) Dimensions.x, ( float ) Dimensions.y ) / 2;
            }
        }

        public Vector3Int [ ] VertexPoints { get; private set; }
        public List < Vector3Int > FloorPoints { get; private set; }


        public void Destroy ( ) {
            foreach ( Vector3Int point in FloorPoints )
                OutputTilemap.SetTile ( point, null );

            Destroy ( gameObject );
        }


        public void Init ( Tilemap outputTilemap, WorldGenTilePlacer tilePlacer, ElementShape shape, Vector3Int dimensions ) {
            this.OutputTilemap = outputTilemap;
            this.TilePlacer = tilePlacer;

            this.Shape = shape;
            this.Dimensions = dimensions;

            GenerateVertexPoints ( roomCoords: transform.position );
        }


        public void GenerateVertexPoints ( Vector3 roomCoords ) {
            transform.position = roomCoords;

            VertexPoints = new Vector3Int [ 0 ];
            FloorPoints = new List < Vector3Int > ( );

            VertexPoints = WorldGenHelper.GenerateShapeVertexPoints ( shape: Shape, centerPoint: Position, dimensions: Dimensions );
  
            DrawGizmo ( );
        }


        public void Tile ( ) {
            for ( int i = 0; i < Dimensions.x; i++ ) {
                for ( int j = 0; j < Dimensions.y; j++ ) {
                    Vector3Int currentPoint = Position + new Vector3Int ( i, j ) - Dimensions / 2;

                    if ( WorldGenHelper.IsPointInsideShape ( point: currentPoint, shapeVertexPoints: VertexPoints ) )
                        FloorPoints.Add ( currentPoint );
                }
            }
      
            TilePlacer.PlaceFloorTiles ( points: FloorPoints.ToArray ( ), outputTilemap: OutputTilemap );
        }


        private void DrawGizmo ( ) {
            ShapeRenderer = GetComponent < ShapeRenderer > ( );

            if ( ShapeRenderer != null ) {
                Vector3 [ ] roomVertexCoords = new Vector3 [ VertexPoints.Length ];
                for ( int i = 0; i < VertexPoints.Length; i++ )
                    roomVertexCoords [ i ] = OutputTilemap.CellToWorld ( VertexPoints [ i ] );

                ShapeRenderer.DrawPolygon ( roomVertexCoords, Color.blue, 0.1f );

                // ShapeRenderer.DrawCircle ( transform.position, RoomRadius * WorldGenHelper.GetTilemapCellRadius ( outputTilemap ), Color.blue, 0.1f );
            }
        }
    }
}
