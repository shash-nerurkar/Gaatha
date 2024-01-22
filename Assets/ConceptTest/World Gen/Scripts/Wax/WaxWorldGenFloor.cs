using UnityEngine;
using Helpers;
using System.Collections.Generic;


namespace WorldGeneration
{
    namespace WaxWorldGeneration
    {
        public class WaxWorldGenFloor : WorldGenFloor
        {
            #region Serialized Fields

            [ SerializeField ] private Vector2Int roomDistFromCenter =  new Vector2Int ( 10, 10 );
            [ SerializeField ] [ Range ( -20, 20 ) ] private int minDistBetweenRooms = -3;

            #endregion

            
            #region Fields

            #endregion


            #region Methods

            public override void Tile ( ) {
                // TilePlacer.PlaceFloorGroundTiles ( points: GroundPoints.ToArray ( ), outputTilemap: OutputTilemap );


                TilePlacer.PlaceFloorWallTiles ( points: WallPoints.ToArray ( ), outputTilemap: OutputTilemap );
            }

            protected override void GenerateWallPoints ( ) {
                List < Vector2Int > unplacedGroundPoints = Vector3IntHelper.ToVector2IntList ( GroundPoints );
                
                foreach ( WorldGenRoom room in Rooms ) {
                    foreach ( Vector3Int roomGroundPoint in room.GroundPoints )
                        unplacedGroundPoints.Remove ( ( Vector2Int ) roomGroundPoint );
                    
                    foreach ( WorldGenCorridor corridor in room.Corridors )
                        foreach ( Vector3Int corridorGroundPoint in corridor.GroundPoints )
                            unplacedGroundPoints.Remove ( ( Vector2Int ) corridorGroundPoint );
                }
                
                foreach ( Vector3Int currentPoint in unplacedGroundPoints )
                    for ( int height = 1; height < WallHeight; height++ )
                        WallPoints.Add ( new Vector3Int ( currentPoint.x, currentPoint.y, GroundPoints [ 0 ].z + height ) );
            }

            protected override bool IsRoomValid ( WorldGenRoom room ) {
                for ( int index = 0; index < Rooms.Count; index++ )
                    if ( !room.Equals ( Rooms [ index ] ) && WorldGenMain.GetDistanceBetweenRooms ( room, Rooms [ index ], subtractRoomRadii: true ) < minDistBetweenRooms )
                        return false;

                return true;
            }

            protected override Vector3Int GetRoomPosition ( WorldGenRoom room ) {
                Vector3Int randomFloorVertexPoint = VertexPoints [ Random.Range ( 0, VertexPoints.Length ) ];
                float tValue = Mathf.Clamp01 ( ( room.Radius + Random.Range ( roomDistFromCenter.x, roomDistFromCenter.y ) ) / Radius );

                return Vector3IntHelper.Vector3IntLerp ( randomFloorVertexPoint, Position, tValue );
            }

            // SET START AND END POINTS 
            public override void SetFloorEndPoints ( ) { }

            #endregion
        }
    }
}
