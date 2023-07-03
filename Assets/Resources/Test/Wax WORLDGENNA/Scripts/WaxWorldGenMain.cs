using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

namespace WaxWorldGeneration
{
    [Serializable]
    public enum ElementShape {
        Quad,
        Circle,
        Hexagon,
        Polygon
    }

    public class WaxWorldGenMain : MonoBehaviour 
    {
        [SerializeField] private GameObject waxRoomPrefab;
        [SerializeField] private Grid outputTilemapGrid;
        [SerializeField] private Tilemap outputTilemap;
        [SerializeField] private TileBase defaultTile;
        private ShapeRenderer shapeRenderer;

        [Header("Floor")]
        [SerializeField] private Vector3Int minimumFloorSize = new Vector3Int( 10, 10 );
        [SerializeField] private Vector3Int maximumFloorSize = new Vector3Int( 20, 20 );
        [SerializeField] [Range(0, 10)] private int maximumFloorElevation = 5;
        
        [Header("Rooms")]
        [SerializeField] private ElementShape[] selectedRoomShapes = new ElementShape[] { ElementShape.Quad };
        [SerializeField] private Vector3Int minimumRoomSize = new Vector3Int( 2, 2 );
        [SerializeField] private Vector3Int maximumRoomSize = new Vector3Int( 5, 5 );
        [SerializeField] [Range(5, 10)] private int minimumRoomCount = 5;
        [SerializeField] [Range(5, 30)] private int maximumRoomCount = 10;
        [SerializeField] [Range(0, 10)] private float maximumDistanceBetweenRooms = 5;
        private List<WaxRoom> rooms = new List<WaxRoom>();
        
        // [Header("Corridors")]
        // [SerializeField] [Range(2, 10)] private int maximumCorridorWidth = 2;
        
        // [Header("Tiles")]
        // [SerializeField] private Vector2 tileSize = new Vector2( 32, 32 );

        // ACTIONS
        public static event Action ClearFloorAction;

        void Awake() {
            transform.position = Vector3.zero;

            GenerateFloor();
        }

        private void ClearFloor() {
            ClearFloorAction?.Invoke();
            
            outputTilemap.ClearAllTiles();

            rooms.Clear();
        }
        
        public void GenerateFloor() {
            ClearFloor();

            CheckInputVariables();

            Vector3Int floorSize = GetRandomVectorBetween( minimumFloorSize, maximumFloorSize );
            Debug.Log( "WAX FLOOR SIZE: " + floorSize );

            int roomCount = Random.Range( minimumRoomCount, maximumRoomCount );
            Debug.Log( "WAX ROOM COUNT: " + roomCount );

            DrawFloorGizmo( floorSize: floorSize );

            for( int i = 0; i < roomCount; i++ ) {
                ElementShape currentRoomShape = SelectRoomShape();
                
                GenerateRoom( roomShape: currentRoomShape, roomIndex: i, floorSize: floorSize );
            }

            CheckAndCreateCorridors();

            SetFloorEndPoints();
        }

        private void CheckInputVariables() {
            if( maximumFloorSize.magnitude < minimumFloorSize.magnitude )
                maximumFloorSize = minimumFloorSize;
            
            if( maximumRoomSize.magnitude < minimumRoomSize.magnitude )
                maximumRoomSize = minimumRoomSize;
            
            if( maximumRoomCount < minimumRoomCount )
                maximumRoomCount = minimumRoomCount;
        }

        private ElementShape SelectRoomShape() => selectedRoomShapes[ Random.Range( 0, selectedRoomShapes.Length ) ];

        private void GenerateRoom( ElementShape roomShape, int roomIndex, Vector3Int floorSize ) {
            Vector3Int roomSize = GetRandomVectorBetween( minimumRoomSize, maximumRoomSize );
            
            Vector2 roomPosition = GetRoomPosition( roomSize: roomSize, roomIndex: roomIndex, floorSize: floorSize );

            int roomElevation = Random.Range( 0, maximumFloorElevation );

            GameObject room = Instantiate( original: waxRoomPrefab, position: roomPosition, rotation: Quaternion.identity, parent: transform );
            
            WaxRoom waxRoomComponent = room.AddComponent<WaxRoom> ();
            waxRoomComponent.InitRoom( outputTilemap: outputTilemap, defaultTile: defaultTile, roomShape: roomShape, roomElevation: roomElevation, roomDimensions: roomSize );
            rooms.Add( waxRoomComponent );
        }

        private Vector2 GetRoomPosition( Vector3Int roomSize, int roomIndex, Vector3Int floorSize ) {
            bool IsRoomPositionValid( Vector2 roomPosition, Vector3Int roomSize, int roomIndex, Vector3Int floorSize ) {
                // CHECK IF SELECTED ROOM POSITION IS BETWEEN THE BOUNDS OF THE FLOOR, FOR THE ROOM SIZE
                return true;
            }
        
            Vector2 roomPosition = Vector2.zero; 
            do {
                Vector2 floorPosition = transform.position;
                float floorWidth = ( outputTilemap.CellToWorld( new Vector3Int( floorSize.x, 0 ) ) - outputTilemap.CellToWorld( new Vector3Int( 0, 0 ) ) ).x;
                float floorHeight = ( outputTilemap.CellToWorld( new Vector3Int( 0, floorSize.y ) ) - outputTilemap.CellToWorld( new Vector3Int( 0, 0 ) ) ).y;
                
                roomPosition = GetRandomVectorBetween(
                    floorPosition - new Vector2( floorWidth, floorHeight ) / 2,
                    floorPosition + new Vector2( floorWidth, floorHeight ) / 2
                );
            } while( !IsRoomPositionValid( roomPosition: roomPosition,  roomSize: roomSize, roomIndex: roomIndex, floorSize: floorSize ) );

            return roomPosition;
        }

        private void CheckAndCreateCorridors() {
            // IF THERE ARE ANY COMPLETELY DISCONNECTED ROOMS, CONNECT THEM TO THE CLOSEST ONE WITH A CORRIDOR
        }

        private void SetFloorEndPoints() {
            // SET START AND END POINTS
        }

        public static Vector2 GetRandomVectorBetween( Vector2 minVector, Vector2 maxVector ) {
            return new Vector2( 
                Random.Range( minVector.x, maxVector.x ),
                Random.Range( minVector.y, maxVector.y )
            );
        }

        public static Vector3Int GetRandomVectorBetween( Vector3Int minVector, Vector3Int maxVector ) {
            return new Vector3Int( 
                Random.Range( minVector.x, maxVector.x ),
                Random.Range( minVector.y, maxVector.y )
            );
        }
        
        private void DrawFloorGizmo( Vector3Int floorSize ) {
            shapeRenderer = GetComponent<ShapeRenderer>();

            if( shapeRenderer != null ) {
                float floorWidth = ( outputTilemap.CellToWorld( new Vector3Int( floorSize.x, 0 ) ) - outputTilemap.CellToWorld( new Vector3Int( 0, 0 ) ) ).x;
                float floorHeight = ( outputTilemap.CellToWorld( new Vector3Int( 0, floorSize.y ) ) - outputTilemap.CellToWorld( new Vector3Int( 0, 0 ) ) ).y;

                shapeRenderer.DrawQuad(
                    center: transform.position,
                    width: floorWidth,
                    height: floorHeight,
                    color: Color.yellow,
                    lineWidth: 0.1f
                );
            }
        }
    }
}