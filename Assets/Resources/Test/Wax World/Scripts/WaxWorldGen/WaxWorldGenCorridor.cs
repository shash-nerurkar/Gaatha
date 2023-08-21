namespace WorldGeneration
{
    namespace WaxWorldGeneration
    {
        public class WaxWorldGenCorridor : WorldGenCorridor
        {
            public override void Tile ( ) {
                TilePlacer.PlaceCorridorGroundTiles ( points: GroundPoints.ToArray ( ), outputTilemap: OutputTilemap );
                                
                                
                // TilePlacer.PlaceCorridorWallTiles ( points: WallPoints.ToArray ( ), outputTilemap: OutputTilemap );
            }
        }
    }
}
