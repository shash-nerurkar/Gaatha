using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace WorldGeneration
{
    public abstract class WorldGenMain : MonoBehaviour 
    {
        [ SerializeField ] private Tilemap outputTilemap;
        [ SerializeField ] private WorldGenTilePlacer tilePlacer;

        public Tilemap OutputTilemap { get { return outputTilemap; } }
        private ShapeRenderer shapeRenderer;


        [ Header ( "Prefabs" ) ]
        [ SerializeField ] private GameObject worldGenRoomPrefab;
        [ SerializeField ] private GameObject worldGenCorridorPrefab;


        [ Header ( "Floor" ) ]
        [ SerializeField ] private ElementShape floorShape = ElementShape.Polygon;
        [ SerializeField ] private Vector3Int minFloorSize = new Vector3Int ( 100, 100, 0 );
        [ SerializeField ] private Vector3Int maxFloorSize = new Vector3Int ( 120, 120, 5 );

        public Vector3Int Position {
            get { return OutputTilemap.WorldToCell ( transform.position ); }
        }
        public Vector3Int Dimensions { get; set; }
        public float Radius {
            get {
                return Mathf.Max ( ( float ) Dimensions.x, ( float ) Dimensions.y ) / 2;
            }
        }
        public Vector3Int[ ] VertexPoints { get; private set; }

        
        [ Header ( "Rooms" ) ]
        [ SerializeField ] private ElementShape [ ] roomShapes = new ElementShape [ ] { ElementShape.Quad };
        [ SerializeField ] private Vector2Int roomSizeRange = new Vector2Int ( 20, 25 );
        [ SerializeField ] private Vector2Int roomCountRange = new Vector2Int ( 10, 15 );

        public List < WorldGenRoom > Rooms { get; private set; }

        
        // [ Header ( "Corridors" ) ]
        // [ SerializeField ] [ Range ( 2, 10 ) ] private int maxCorridorWidth = 2;

        
        [ Header ( "Misc" ) ]
        [ SerializeField ] private GameObject dummyPrefab;
        [ SerializeField ] [ Range ( 1, 500 ) ] private int roomSpawnIterations = 100;

        public int RoomSpawnIterations { get { return roomSpawnIterations; } }
        public List < GameObject > DummyPrefabs { get; private set; }
        public GameObject DummyPrefab { get { return dummyPrefab; } }



        void Awake ( ) {
            Rooms = new List < WorldGenRoom > ( );
            DummyPrefabs = new List < GameObject > ( );
            
            transform.position = Vector3.zero;

            GenerateFloor ( );
        }



        private void ClearFloor ( ) {
            VertexPoints = new Vector3Int[0];
            
            OutputTilemap.ClearAllTiles ( );

            foreach ( WorldGenRoom room in Rooms )
                room.Destroy ( );
            Rooms.Clear ( );

            foreach ( GameObject dummyPrefab in DummyPrefabs )
                GameObject.Destroy ( dummyPrefab );
            DummyPrefabs.Clear ( );
        }
        
        private void CheckInputVariables ( ) {
            if ( maxFloorSize.x  <  minFloorSize.x )
                maxFloorSize.x = minFloorSize.x;
            
            if ( maxFloorSize.y  <  minFloorSize.y )
                maxFloorSize.y = minFloorSize.y;
            
            if ( roomSizeRange.y  <  roomSizeRange.x )
                roomSizeRange.y = roomSizeRange.x;
            
            if ( roomCountRange.y  <  roomCountRange.x )
                roomCountRange.y = roomCountRange.x;
            
            if( roomShapes.Length == 0 )
                roomShapes = new ElementShape[1] { ElementShape.Quad };
        }

        public void GenerateFloor ( ) {
            ClearFloor ( );

            CheckInputVariables ( );

            Dimensions = ( Vector3Int ) WorldGenHelper.GetRandomVectorBetween ( new Vector3Int ( minFloorSize.x, minFloorSize.y, 0 ), new Vector3Int ( maxFloorSize.x, maxFloorSize.y, 0 ) );
            Debug.Log ( "WAX FLOOR SIZE: " + Dimensions );

            VertexPoints = WorldGenHelper.GenerateShapeVertexPoints ( shape: floorShape, centerPoint: Vector3Int.zero, dimensions: Dimensions );

            int roomCount = Random.Range ( roomCountRange.x, roomCountRange.y );
            Debug.Log ( "WAX ROOM COUNT: " + roomCount );

            DrawGizmo (  );

            for ( int index = 0; index < roomCount; index++ ) {
                bool didRoomGenerate = GenerateRoom ( );

                if ( !didRoomGenerate ) {
                    Debug.Log ( "ROOM GENERATION HALTED AT INDEX: " + index );
                    // if ( index  <  roomCountRange.x - 1 ) {
                    //     Debug.Log ( "REGENERATING FLOOR AUTOMATICALLY" );
                    //     GenerateFloor ( );
                    //     return;
                    // }
                    break;
                }
            }

            CheckAndCreateCorridors ( );

            SetFloorEndPoints ( );
        }



        private bool GenerateRoom ( ) {
            ElementShape roomShape = roomShapes [ Random.Range ( 0, roomShapes.Length ) ];
            
            Vector2Int roomSize = new Vector2Int ( Random.Range ( roomSizeRange.x, roomSizeRange.y ), Random.Range ( roomSizeRange.x, roomSizeRange.y ) );
           
            Vector3 roomCoords = new Vector3 ( 0, 0, Random.Range ( minFloorSize.z, maxFloorSize.z ) );

            GameObject roomObj = Instantiate ( original: worldGenRoomPrefab, position: roomCoords, rotation: Quaternion.identity, parent: transform );
            
            WorldGenRoom worldGenRoomComponent = roomObj.GetComponent < WorldGenRoom > ( );
            worldGenRoomComponent.Init ( outputTilemap: OutputTilemap, tilePlacer: tilePlacer, shape: roomShape, dimensions: ( Vector3Int ) roomSize );
            
            bool areRoomCoordsValid = SetRoomCoords ( room: worldGenRoomComponent );
            
            if ( areRoomCoordsValid ) {
                worldGenRoomComponent.Tile ( );
                Rooms.Add ( worldGenRoomComponent );
            }
            else {
                worldGenRoomComponent.Destroy ( );
            }

            return areRoomCoordsValid;
        }

        protected virtual bool IsRoomValid ( WorldGenRoom room ) => WorldGenHelper.IsShapeInsideShape ( room.VertexPoints, VertexPoints );

        public abstract bool SetRoomCoords ( WorldGenRoom room );

        public float GetDistanceBetweenRooms ( WorldGenRoom room1, WorldGenRoom room2, bool subtractRoomRadii = false ) {
            float roomRadii = room1.Radius + room2.Radius;

            return ( Vector3Int.Distance ( room1.Position, room2.Position ) - ( subtractRoomRadii ? roomRadii : 0 ) );
        }
        
        public WorldGenRoom GetClosestRoom ( WorldGenRoom room ) {
            if ( Rooms == null || Rooms.Count  <  0 )
                throw new UnassignedReferenceException ( "Floor Rooms array is empty/null." );
            
            float distBetweenRooms = GetDistanceBetweenRooms ( Rooms [ 0 ], room, subtractRoomRadii: true );
            WorldGenRoom resultRoom = Rooms [ 0 ];

            foreach ( WorldGenRoom otherRoom in Rooms ) {
                float distBetweenCurrentRooms = GetDistanceBetweenRooms ( otherRoom, room, subtractRoomRadii: true );
                
                if ( distBetweenCurrentRooms  <  distBetweenRooms ) {
                    distBetweenRooms = distBetweenCurrentRooms;
                    resultRoom = otherRoom;
                }
            }

            return resultRoom;
        }



        public void CheckAndCreateCorridors ( ) {
            foreach ( WorldGenRoom room in Rooms ) {
                bool isRoomSeparate = true;

                foreach ( WorldGenRoom otherRoom in Rooms )
                    if( WorldGenHelper.IsShapeIntersectingShape ( room.VertexPoints, otherRoom.VertexPoints, firstShapeCenterPoint: room.Position ) ) {
                        isRoomSeparate = false;
                        break;
                    }

                if ( isRoomSeparate )
                    SetRoomCorridors ( room: room );
            }
        }

        public abstract void SetRoomCorridors ( WorldGenRoom room );



        public abstract void SetFloorEndPoints ( );



        private void DrawGizmo ( ) {
            shapeRenderer = GetComponent < ShapeRenderer >  ( );

            if ( shapeRenderer != null ) {
                Vector3[ ] floorVertexCoords = new Vector3[ VertexPoints.Length ];
                for( int i = 0; i  <  VertexPoints.Length; i++ )
                    floorVertexCoords[ i ] = OutputTilemap.CellToWorld ( VertexPoints[ i ] );

                shapeRenderer.DrawPolygon ( floorVertexCoords, Color.blue, 0.1f );

                // shapeRenderer.DrawCircle ( transform.position, Radius * WorldGenHelper.GetTilemapCellRadius ( outputTilemap ), Color.blue, 0.1f );
            }
        }
    }
}