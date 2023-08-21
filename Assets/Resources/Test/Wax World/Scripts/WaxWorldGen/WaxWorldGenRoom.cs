namespace WorldGeneration
{
    namespace WaxWorldGeneration
    {
        public class WaxWorldGenRoom : WorldGenRoom
        {
            public override void Tile ( ) {
                TilePlacer.PlaceRoomGroundTiles ( points: GroundPoints.ToArray ( ), outputTilemap: OutputTilemap );


                // TilePlacer.PlaceRoomWallTiles ( points: WallPoints.ToArray ( ), outputTilemap: OutputTilemap );
            }
        }
    }
}
