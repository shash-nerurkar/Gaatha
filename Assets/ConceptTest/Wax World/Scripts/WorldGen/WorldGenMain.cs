using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Helpers;

namespace WorldGeneration
{
    public abstract class WorldGenMain : MonoBehaviour 
    {
        #region Serialized Fields

        [ SerializeField ] private Tilemap mainTilemap;
        [ SerializeField ] private WorldGenTilePlacer tilePlacer;

        [ Header ( "Enclosure Prefabs" ) ]
        [ SerializeField ] private GameObject floorPrefab;
        [ SerializeField ] private GameObject roomPrefab;
        [ SerializeField ] private GameObject corridorPrefab;

        [ Header ( "Floor" ) ]
        [ SerializeField ] private Vector2Int floorCountRange = new ( 10, 15 );
        [ SerializeField ] private ElementShape floorShape = ElementShape.RegularPolygon;
        [ SerializeField ] private Vector2Int minFloorSize = new ( 100, 100 );
        [ SerializeField ] private Vector2Int maxFloorSize = new ( 120, 120 );
        [ SerializeField ] private Vector2Int floorWallHeightRange = new ( 10, 20 );
        
        [ Header ( "Floor Elevation" ) ]
        [ SerializeField ] [ Range ( 20, 40 ) ] private int elevationMultiplier = 25;

        #endregion


        #region Fields

        public List < WorldGenFloor > Floors { get; private set; }

        #endregion


        #region Methods

        void Awake ( ) {
            Floors = new List< WorldGenFloor > ( );

            GenerateMap ( );
        }

        private void ClearMap ( ) {
            mainTilemap.ClearAllTiles ( );

            foreach ( WorldGenFloor floor in Floors ) {
                floor.OutputTilemap.ClearAllTiles ( );
                foreach ( WorldGenRoom room in floor.Rooms ) {
                    room.OutputTilemap.ClearAllTiles ( );

                    foreach ( WorldGenCorridor corridor in room.Corridors )
                        corridor.OutputTilemap.ClearAllTiles ( );
                }

                Destroy ( floor.gameObject );
            }
            Floors.Clear ( );
        }

        public void GenerateMap ( ) {
            ClearMap ( );


            int floorCount = Random.Range ( floorCountRange.x, floorCountRange.y );
            Debug.Log ( "WAX FLOOR COUNT: " + floorCount );

            for ( int index = 0; index < floorCount; index++ ) {
                WorldGenFloor floor = GenerateFloor ( index );
                Floors.Add ( floor );

                floor.GenerateRooms ( roomPrefab: roomPrefab, corridorPrefab: corridorPrefab );

                floor.GeneratePoints ( floor.transform.position );

                tilePlacer.Init ( floor );

                floor.Tile ( );
                foreach ( WorldGenRoom room in floor.Rooms ) {
                    room.Tile ( );

                    foreach ( WorldGenCorridor corridor in room.Corridors )
                        corridor.Tile ( );
                }

                floor.SetFloorEndPoints ( );
            }
        }

        public WorldGenFloor GenerateFloor ( int index ) {
            int floorWallHeight = Random.Range ( floorWallHeightRange.x, floorWallHeightRange.y );
            Vector3Int floorSize = ( Vector3Int ) Vector3IntHelper.GetRandomVectorBetween ( minFloorSize, maxFloorSize );
            Vector3Int floorPos = new ( 0, 0, index * elevationMultiplier );
            Vector3 floorCoords = transform.position + mainTilemap.CellToWorld ( floorPos );

            GameObject floorObj = Instantiate ( original: floorPrefab, position: floorCoords, rotation: Quaternion.identity, parent: transform );
            floorObj.name = "Floor " + index;
            
            WorldGenFloor floor = floorObj.GetComponent < WorldGenFloor > ( );
            floor.Init ( mainTilemap: mainTilemap, tilePlacer: tilePlacer, shape: floorShape, dimensions: floorSize, wallHeight: floorWallHeight );

            return floor;
        }

        public static float GetDistanceBetweenRooms ( WorldGenRoom room1, WorldGenRoom room2, bool subtractRoomRadii = false ) {
            float roomRadii = room1.Radius + room2.Radius;

            return Vector3Int.Distance ( room1.Position, room2.Position ) - ( subtractRoomRadii ? roomRadii : 0 );
        }

        #endregion
    }
}