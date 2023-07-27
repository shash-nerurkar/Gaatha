using UnityEngine;

namespace WorldGeneration
{
    namespace WaxWorldGeneration
    {
        public class Element2WorldGenMain : WorldGenMain
        {
            protected override bool IsRoomValid ( WorldGenRoom room ) {
                if ( !base.IsRoomValid ( room ) )
                    return false;

                foreach ( WorldGenRoom otherRoom in Rooms )
                    if( WorldGenHelper.IsShapeIntersectingShape ( room.VertexPoints, otherRoom.VertexPoints, firstShapeCenterPoint: room.Position ) )
                        return false;

                return true;
            }

            public override bool SetRoomCoords ( WorldGenRoom room ) {
                int maxIterations = RoomSpawnIterations;
                Vector3 roomCoords = room.transform.position;
                int roomElevation = ( int ) roomCoords.z;

                while ( !IsRoomValid ( room ) && maxIterations-- > 0 ) {
                    Vector3Int randomPositionOnFloor = WorldGenHelper.GetRandomPointInsideShape ( vertices: VertexPoints, outputTilemap: OutputTilemap );
                    randomPositionOnFloor.z = roomElevation;
                    roomCoords = OutputTilemap.CellToWorld ( randomPositionOnFloor );

                    room.GenerateVertexPoints ( roomCoords: roomCoords );
                }

                return IsRoomValid ( room );
            }
         
         

            // CONNECT THEM TO THE CLOSEST ONE WITH A CORRIDOR
            public override void SetRoomCorridors ( WorldGenRoom room ) { }



            // SET START AND END POINTS 
            public override void SetFloorEndPoints ( ) { }
        }
    }
}
