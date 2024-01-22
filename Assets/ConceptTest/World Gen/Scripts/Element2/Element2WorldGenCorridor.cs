namespace WorldGeneration
{
    namespace Element2WorldGeneration
    {
        public class Element2WorldGenCorridor : WorldGenCorridor
        {
            public override void Tile ( ) {
                TilePlacer.PlaceCorridorGroundTiles ( points: GroundPoints.ToArray ( ), outputTilemap: OutputTilemap );
                                
                                
                TilePlacer.PlaceCorridorWallTiles ( points: WallPoints.ToArray ( ), outputTilemap: OutputTilemap );
            }
        }
    }
}
