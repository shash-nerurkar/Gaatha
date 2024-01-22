using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace WorldGeneration
{
    public abstract class WorldGenEnclosure : MonoBehaviour
    {
        #region Serialized Fields

        #endregion


        #region Fields

        public ShapeRenderer ShapeRenderer { get; private set; }
        public Tilemap OutputTilemap { get; private set; }
        public WorldGenTilePlacer TilePlacer { get; private set; }
        public Tilemap MainTilemap { get; private set; }
        
        public ElementShape Shape { get; private set; }
        public Vector3Int Position {
            get { return MainTilemap.WorldToCell ( transform.position ); }
        }
        public Vector3Int Dimensions { get; private set; }
        public float Radius {
            get { 
                return Mathf.Max ( Dimensions.x, ( float ) Dimensions.y ) / 2;
            }
        }
        public int WallHeight { get; private set; }

        public Vector3Int [ ] VertexPoints { get; set; }
        public List < Vector3Int > GroundPoints { get; set; }
        public List < Vector3Int > WallPoints { get; set; }

        #endregion


        #region Methods

        public void Init ( Tilemap mainTilemap, WorldGenTilePlacer tilePlacer, ElementShape shape, Vector3Int dimensions, int wallHeight ) {
            OutputTilemap = GetComponentInChildren < Tilemap > ( );
            TilePlacer = tilePlacer;
            MainTilemap = mainTilemap;
            
            Shape = shape;
            Dimensions = dimensions;
            WallHeight = wallHeight;

            GeneratePoints ( coords: transform.position );
        }

        public void GeneratePoints ( Vector3 coords ) {
            transform.position = coords;
            
            VertexPoints = new Vector3Int [ 0 ];
            GenerateVertexPoints ( );

            GroundPoints = new List < Vector3Int > ( );
            GenerateGroundPoints ( );

            WallPoints = new List < Vector3Int > ( );
            GenerateWallPoints ( );

            DrawGizmo ( );
        }

        protected virtual void GenerateVertexPoints ( ) {
            VertexPoints = WorldGenHelper.GenerateShapeVertexPoints ( shape: Shape, position: Vector3Int.zero, dimensions: Dimensions );
        }

        protected virtual void GenerateGroundPoints ( ) {
            for ( int i = 0; i < Dimensions.x; i++ ) {
                for ( int j = 0; j < Dimensions.y; j++ ) {
                    Vector3Int currentPoint = new Vector3Int ( i, j ) - Dimensions / 2;

                    if ( WorldGenHelper.IsPointInsideShape ( point: currentPoint, shapeVertexPoints: VertexPoints ) )
                        GroundPoints.Add ( currentPoint );
                }
            }
        }

        protected virtual void GenerateWallPoints ( ) {
            for ( int i = 0; i < VertexPoints.Length - 1; i++ ) {
                Vector3Int firstVertex = VertexPoints [ i ];
                Vector3Int secondVertex = VertexPoints [ i + 1 ];

                foreach ( Vector3Int currentPoint in WorldGenHelper.GetPointsBetween ( firstVertex, secondVertex ) )
                    if ( !WallPoints.Contains ( new Vector3Int ( currentPoint.x, currentPoint.y, currentPoint.z + 1 ) ) )
                        for ( int height = 1; height < WallHeight; height++ )
                            WallPoints.Add ( new Vector3Int ( currentPoint.x, currentPoint.y, currentPoint.z + height ) );
            }
        }

        public abstract void Tile ( );

        private void DrawGizmo ( ) {
            ShapeRenderer = GetComponent < ShapeRenderer > ( );

            if ( ShapeRenderer != null ) {
                Vector3 [ ] roomVertexCoords = new Vector3 [ VertexPoints.Length ];
                for ( int i = 0; i < VertexPoints.Length; i++ )
                    roomVertexCoords [ i ] = OutputTilemap.CellToWorld ( VertexPoints [ i ] );

                ShapeRenderer
                    .DrawPolygon ( roomVertexCoords, Color.blue, 0.1f );
                    // .DrawCircle ( transform.position, RoomRadius * WorldGenHelper.GetTilemapCellRadius ( MainTilemap ), Color.blue, 0.1f );
            }
        }

        #endregion
    }
}
