using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace WaxWorldGeneration
{
    public class WaxWorldGenMain : MonoBehaviour 
    {
        [SerializeField] private GameObject waxRoomPrefab;
        [SerializeField] private Tilemap outputTilemap;
        private WaxTilePlacer tilePlacer;
        private ShapeRenderer shapeRenderer;


        [Header ("Floor")]
        [SerializeField] private ElementShape floorShape = ElementShape.Quad;
        [SerializeField] private Vector3Int minFloorSize = new Vector3Int ( 100, 100, 0 );
        [SerializeField] private Vector3Int maxFloorSize = new Vector3Int ( 120, 120, 5 );
        private Vector3Int[] FloorVertexPoints;
        private Vector3Int floorSize;

        
        [Header ("Rooms")]
        [SerializeField] private ElementShape[] roomShapes = new ElementShape[] { ElementShape.Quad };
        [SerializeField] private Vector2Int roomSizeRange = new Vector2Int ( 10, 20 );
        [SerializeField] private Vector2Int roomCountRange = new Vector2Int ( 5, 15 );
        // [SerializeField] [Range ( 0, 10 )] private float maxDistBetweenRooms = 5;
        private List<WaxRoom> rooms = new List<WaxRoom> ();

        
        // [Header ("Corridors")]
        // [SerializeField] [Range (2, 10 )] private int maxCorridorWidth = 2;


        void Awake () {
            transform.position = Vector3.zero;

            tilePlacer = GetComponent<WaxTilePlacer> ();

            GenerateFloor ();
        }

        [SerializeField] private GameObject dummyPrefab;
        private List<GameObject> dummyPrefabs = new List<GameObject> ();

        private void ClearFloor () {
            FloorVertexPoints = new Vector3Int[0];
            
            outputTilemap.ClearAllTiles ();

            foreach ( WaxRoom room in rooms )
                room.DestroyRoom ();
            rooms.Clear ();

            foreach ( GameObject dummyPrefab in dummyPrefabs )
                GameObject.Destroy ( dummyPrefab );
            dummyPrefabs.Clear ();
        }
        
        private void CheckInputVariables () {
            if ( maxFloorSize.x < minFloorSize.x )
                maxFloorSize.x = minFloorSize.x;
            
            if ( maxFloorSize.y < minFloorSize.y )
                maxFloorSize.y = minFloorSize.y;
            
            if ( roomSizeRange.y < roomSizeRange.x )
                roomSizeRange.y = roomSizeRange.x;
            
            if ( roomCountRange.y < roomCountRange.x )
                roomCountRange.y = roomCountRange.x;
            
            if( roomShapes.Length == 0 )
                roomShapes = new ElementShape[1] { ElementShape.Quad };
        }

        public void GenerateFloor () {
            ClearFloor ();

            CheckInputVariables ();

            floorSize = ( Vector3Int ) WorldGenHelper.GetRandomVectorBetween ( ( Vector2Int ) minFloorSize, ( Vector2Int ) maxFloorSize );
            Debug.Log ( "WAX FLOOR SIZE: " + floorSize );

            FloorVertexPoints = WorldGenHelper.GenerateShapeVertexPoints ( shape: floorShape, centerPoint: Vector3Int.zero, dimensions: floorSize );

            int roomCount = Random.Range ( roomCountRange.x, roomCountRange.y );
            Debug.Log ( "WAX ROOM COUNT: " + roomCount );

            DrawFloorGizmo ( floorSize: floorSize );

            for ( int index = 0; index < roomCount; index++ ) {
                bool didRoomGenerate = GenerateRoom ();

                if( !didRoomGenerate ) {
                    Debug.Log( "ROOM GENERATION HALTED AT INDEX: " + index );
                    break;
                }
            }

            CheckAndCreateCorridors ();

            SetFloorEndPoints ();
        }

        private bool GenerateRoom () {
            ElementShape roomShape = roomShapes[ Random.Range ( 0, roomShapes.Length ) ];
            Vector2Int roomSize = new Vector2Int ( Random.Range ( roomSizeRange.x, roomSizeRange.y ), Random.Range ( roomSizeRange.x, roomSizeRange.y ) );
            Vector3 roomCoords = new Vector3 ( 0, 0, 0 ); // Random.Range ( minFloorSize.z, maxFloorSize.z ) );

            GameObject roomObj = Instantiate ( original: waxRoomPrefab, position: roomCoords, rotation: Quaternion.identity, parent: transform );
            WaxRoom waxRoomComponent = roomObj.AddComponent<WaxRoom> ();
            waxRoomComponent.InitRoom ( outputTilemap: outputTilemap, tilePlacer: tilePlacer, roomShape: roomShape, roomDimensions: ( Vector3Int ) roomSize );
            
            bool areRoomCoordsValid = SetRoomCoords ( room: waxRoomComponent );
            
            if ( areRoomCoordsValid ) {
                waxRoomComponent.TileRoom ();
                rooms.Add ( waxRoomComponent );
            }
            else {
                waxRoomComponent.DestroyRoom ();
            }

            return areRoomCoordsValid;
        }

        private bool SetRoomCoords ( WaxRoom room ) {
            bool IsRoomValid ( WaxRoom room ) {
                if( !WorldGenHelper.IsShapeInsideShape ( room.RoomVertexPoints, FloorVertexPoints ) )
                    return false;

                foreach ( WaxRoom otherRoom in rooms )
                    if( WorldGenHelper.IsShapeIntersectingShape ( room.RoomVertexPoints, otherRoom.RoomVertexPoints, firstShapeCenterPoint: room.RoomPosition ) )
                        return false;

                return true;
            }

            int maxIterations = 100;
            bool isRoomValid = IsRoomValid ( room );
            Vector3 roomCoords = room.transform.position;
            int roomElevation = ( int ) roomCoords.z;

            while ( !isRoomValid && maxIterations-- > 0 ) {
                GameObject dummyTestObj = Instantiate ( original: dummyPrefab, position: roomCoords, rotation: Quaternion.identity );
                dummyPrefabs.Add ( dummyTestObj );

                Vector3Int randomPositionOnFloor = WorldGenHelper.GetRandomPointInsideShape ( vertices: FloorVertexPoints, outputTilemap: outputTilemap );
                randomPositionOnFloor.z = roomElevation;
                roomCoords = outputTilemap.CellToWorld ( randomPositionOnFloor );

                room.GenerateRoomVertexPoints ( roomCoords: roomCoords );

                isRoomValid = IsRoomValid ( room );
            }

            return isRoomValid;
        }

        private void CheckAndCreateCorridors () {
            // IF THERE ARE ANY COMPLETELY DISCONNECTED ROOMS, CONNECT THEM TO THE CLOSEST ONE WITH A CORRIDOR
        }

        private void SetFloorEndPoints () {
            // SET START AND END POINTS
        }

        private void DrawFloorGizmo ( Vector3Int floorSize ) {
            shapeRenderer = GetComponent<ShapeRenderer> ();

            if ( shapeRenderer != null ) {
                Vector3[] floorVertexCoords = new Vector3[ FloorVertexPoints.Length ];
                for( int i = 0; i < FloorVertexPoints.Length; i++ )
                    floorVertexCoords[i] = outputTilemap.CellToWorld( FloorVertexPoints[ i ] );

                shapeRenderer.DrawPolygon( floorVertexCoords, Color.blue, 0.1f );
            }
        }
    }
}