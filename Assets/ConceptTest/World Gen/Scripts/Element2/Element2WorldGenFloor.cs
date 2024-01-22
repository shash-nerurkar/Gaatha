using UnityEngine;

namespace WorldGeneration
{
    namespace Element2WorldGeneration
    {
        public class Element2WorldGenFloor : WorldGenFloor
        {
            public override void Tile ( ) {
                // TilePlacer.PlaceFloorGroundTiles ( points: GroundPoints.ToArray ( ), outputTilemap: OutputTilemap );


                // TilePlacer.PlaceFloorWallTiles ( points: WallPoints.ToArray ( ), outputTilemap: OutputTilemap );
            }



            protected override bool IsRoomValid ( WorldGenRoom room ) {
                if ( !base.IsRoomValid ( room ) )
                    return false;

                foreach ( WorldGenRoom otherRoom in Rooms )
                    if( WorldGenHelper.IsShapeIntersectingShape ( room.VertexPoints, otherRoom.VertexPoints, firstShapeCenterPoint: room.Position ) )
                        return false;

                return true;
            }
        }
    }
}
