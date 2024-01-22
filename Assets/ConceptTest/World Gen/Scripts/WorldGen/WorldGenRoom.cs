using System.Collections.Generic;
using UnityEngine;


namespace WorldGeneration
{
    public abstract class WorldGenRoom : WorldGenEnclosure
    {
        #region Serialized Fields

        // [ SerializeField ] [ Range ( 2, 20 ) ] private int maxCorridorWidth = 6;

        #endregion


        #region Fields

        public List < WorldGenCorridor > Corridors { get; private set; }

        #endregion


        #region Methods

        void Awake ( ) => Corridors = new List < WorldGenCorridor > ( );
        
        public void GenerateCorridors ( GameObject corridorPrefab, List < WorldGenRoom > rooms ) {
            foreach ( WorldGenRoom otherRoom in rooms )
                if( !Equals ( otherRoom ) && WorldGenHelper.IsShapeIntersectingShape ( VertexPoints, otherRoom.VertexPoints, Position ) )
                    return;
            
            WorldGenRoom closestRoom = GetClosestRoom ( rooms );
            
            WorldGenCorridor corridor = GenerateCorridor ( corridorPrefab, closestRoom );       
            Corridors.Add ( corridor );
        }

        private WorldGenCorridor GenerateCorridor ( GameObject corridorPrefab, WorldGenRoom closestRoom ) {
            ElementShape corridorShape = ElementShape.Quad;
            Vector3Int corridorSize = new Vector3Int (
                Mathf.Abs ( Position.x - closestRoom.Position.x ),
                Mathf.Abs ( Position.y - closestRoom.Position.y ),
                Position.z
            );       
            Vector3Int corridorPos = Vector3Int.zero;
            Vector3 corridorCoords = transform.position + MainTilemap.CellToWorld ( corridorPos );

            GameObject corridorObj = Instantiate ( original: corridorPrefab, position: corridorCoords, rotation: Quaternion.identity, parent: transform );
            corridorObj.name = "Corridor";

            WorldGenCorridor corridor = corridorObj.GetComponent < WorldGenCorridor > ( );
            corridor.SetEndPoints ( Position, closestRoom.Position );
            corridor.Init ( mainTilemap: MainTilemap, tilePlacer: TilePlacer, shape: corridorShape, dimensions: corridorSize, wallHeight: WallHeight );
            
            return corridor;
        }

        public WorldGenRoom GetClosestRoom ( List < WorldGenRoom > rooms ) {
            if ( rooms == null || rooms.Count < 1 )
                throw new UnassignedReferenceException ( "Floor Rooms array is empty/null." );
            
            float distBetweenRooms = Mathf.Infinity;
            WorldGenRoom resultRoom = rooms [ 0 ];

            foreach ( WorldGenRoom otherRoom in rooms ) {
                float distBetweenCurrentRooms = WorldGenMain.GetDistanceBetweenRooms ( this, otherRoom, subtractRoomRadii: true );
                
                if ( !Equals ( otherRoom ) && distBetweenCurrentRooms < distBetweenRooms ) {
                    distBetweenRooms = distBetweenCurrentRooms;
                    resultRoom = otherRoom;
                }
            }

            return resultRoom;
        }

        public override void Tile ( ) {
            TilePlacer.PlaceRoomGroundTiles ( points: GroundPoints.ToArray ( ), outputTilemap: OutputTilemap );

            TilePlacer.PlaceRoomWallTiles ( points: WallPoints.ToArray ( ), outputTilemap: OutputTilemap );
        }

        #endregion
    }
}
