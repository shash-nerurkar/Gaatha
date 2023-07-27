using UnityEngine;

namespace WorldGeneration
{
    namespace WaxWorldGeneration
    {
        public class WaxWorldGenMain : WorldGenMain
        {
            [ SerializeField ] [ Range ( -20, 20 ) ] private int minDistBetweenRooms = 5;


            protected override bool IsRoomValid ( WorldGenRoom room ) {
                for ( int index = 0; index < Rooms.Count; index++ )
                    if ( GetDistanceBetweenRooms ( Rooms [ index ], room, subtractRoomRadii: true ) < minDistBetweenRooms )
                        return false;

                return true;
            }

            public override bool SetRoomCoords ( WorldGenRoom room ) {
                int maxIterations = RoomSpawnIterations;

                Vector3 roomCoords = room.transform.position;
                int roomElevation = ( int ) roomCoords.z;

                while ( !IsRoomValid ( room ) && maxIterations-- > 0 ) {
                    GameObject dummyTestObj = Instantiate ( original: DummyPrefab, position: roomCoords, rotation: Quaternion.identity );
                    DummyPrefabs.Add ( dummyTestObj );

                    Vector3Int randomFloorVertexPoint = VertexPoints [ Random.Range ( 0, VertexPoints.Length ) ];
                    float tValue = Mathf.Clamp01 ( ( room.Radius + Random.Range ( 0, 2 ) ) / Radius );
                    Vector3Int positionOnFloor = WorldGenHelper.Vector3IntLerp ( randomFloorVertexPoint, Position, tValue );

                    positionOnFloor.z = roomElevation;
                    roomCoords = OutputTilemap.CellToWorld ( positionOnFloor );

                    room.GenerateVertexPoints ( roomCoords: roomCoords );
                }

                return IsRoomValid ( room );
            }
         


            public override void SetRoomCorridors ( WorldGenRoom room ) {
                WorldGenRoom closestRoom = base.GetClosestRoom ( room );

                
            }



            // SET START AND END POINTS 
            public override void SetFloorEndPoints ( ) { }
        }
    }
}
