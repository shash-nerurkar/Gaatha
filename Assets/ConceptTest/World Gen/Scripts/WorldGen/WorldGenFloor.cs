using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration
{
    public abstract class WorldGenFloor : WorldGenEnclosure
    {
        #region Serialized Fields

        [ SerializeField ] private ElementShape [ ] roomShapes = new ElementShape [ ] { ElementShape.Circle };
        [ SerializeField ] private Vector2Int roomSizeRange = new ( 25, 30 );
        [ SerializeField ] private Vector2Int roomCountRange = new ( 10, 15 );
        [ SerializeField ] private Vector2Int roomWallHeightRange = new ( 5, 8 );
        
        [ Header ( "Room Elevation" ) ]
        [ SerializeField ] [ Range ( 3, 10 ) ] private int elevationMultiplier = 5;
        [ SerializeField ] private Vector2Int roomElevationRange = new ( 0, 0 );

        [ Header ( "Misc" ) ]
        [ SerializeField ] [ Range ( 1, 500 ) ] private int roomSpawnIterations = 100;
        
        public List < WorldGenRoom > Rooms { get; private set; }
        public int RoomsWallHeight { get; private set; }

        #endregion


        #region Fields

        #endregion

        
        #region Methods

        void Awake ( ) => Rooms = new List < WorldGenRoom > ( );

        public void GenerateRooms ( GameObject roomPrefab, GameObject corridorPrefab ) {
            int roomCount = Random.Range ( roomCountRange.x, roomCountRange.y );
            Debug.Log ( "WAX ROOM COUNT: " + roomCount );
            
            RoomsWallHeight = Random.Range ( roomWallHeightRange.x, roomWallHeightRange.y );

            for ( int index = 0; index < roomCount; index++ ) {
                bool didRoomGenerate = GenerateRoom ( roomPrefab, index, out WorldGenRoom room );

                if ( didRoomGenerate ) {
                    Rooms.Add ( room );

                    // if ( index > 0 )
                    //     Rooms [ index ].GenerateCorridors ( corridorPrefab, Rooms );
                }
                else {
                    Destroy ( room.gameObject );

                    Debug.Log ( "ROOM GENERATION HALTED AT INDEX: " + index );                  
                    break;
                }
            }
        }

        private bool GenerateRoom ( GameObject roomPrefab, int index, out WorldGenRoom room ) {
            ElementShape roomShape = roomShapes [ Random.Range ( 0, roomShapes.Length ) ];     
            Vector2Int roomSize = new ( Random.Range ( roomSizeRange.x, roomSizeRange.y ), Random.Range ( roomSizeRange.x, roomSizeRange.y ) );
            Vector3Int roomPos = new ( 0, 0, Random.Range ( roomElevationRange.x, roomElevationRange.y ) * elevationMultiplier );
            Vector3 roomCoords = transform.position + MainTilemap.CellToWorld ( roomPos );
            
            GameObject roomObj = Instantiate ( original: roomPrefab, position: roomCoords, rotation: Quaternion.identity, parent: transform );
            roomObj.name = "Room " + index;

            room = roomObj.GetComponent < WorldGenRoom > ( );
            room.Init ( mainTilemap: MainTilemap, tilePlacer: TilePlacer, shape: roomShape, dimensions: ( Vector3Int ) roomSize, wallHeight: RoomsWallHeight );
            
            return SetRoomCoords ( room );
        }
        
        private bool SetRoomCoords ( WorldGenRoom room ) {
            int maxIterations = roomSpawnIterations;

            int roomElevation = MainTilemap.WorldToCell ( room.transform.position ).z;

            while ( !IsRoomValid ( room ) && maxIterations-- > 0 ) {
                Vector3Int positionOnFloor = GetRoomPosition ( room );
                positionOnFloor.z = roomElevation;

                room.GeneratePoints ( MainTilemap.CellToWorld ( positionOnFloor ) );
            }

            return IsRoomValid ( room );
        }

        protected virtual bool IsRoomValid ( WorldGenRoom room ) {
            return WorldGenHelper.IsShapeInsideShape ( room.VertexPoints, VertexPoints );
        }

        protected virtual Vector3Int GetRoomPosition ( WorldGenRoom room ) {
            return WorldGenHelper.GetRandomPointInsideShape ( vertices: VertexPoints, outputTilemap: MainTilemap );
        }
     
        public virtual void SetFloorEndPoints ( ) { }

        public override void Tile ( ) {
            TilePlacer.PlaceFloorGroundTiles ( points: GroundPoints.ToArray ( ), outputTilemap: OutputTilemap );

            TilePlacer.PlaceFloorWallTiles ( points: WallPoints.ToArray ( ), outputTilemap: OutputTilemap );
        }

        #endregion
    }
}
