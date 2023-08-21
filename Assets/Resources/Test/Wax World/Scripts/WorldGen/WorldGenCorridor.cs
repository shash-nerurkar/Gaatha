using Helpers;
using UnityEngine;

namespace WorldGeneration
{
    public abstract class WorldGenCorridor : WorldGenEnclosure
    {
        public Vector3Int StartPoint { get; private set; }
        public Vector3Int EndPoint { get; private set; }

        public void SetEndPoints ( Vector3Int startPoint, Vector3Int endPoint ) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
        
        protected override void GenerateVertexPoints ( ) {
            VertexPoints = WorldGenHelper.GetPointsBetween ( StartPoint, EndPoint );
            Debug.Log ( "VertexPoints: " + Vector3IntHelper.ArrayToString ( VertexPoints ) );
        }

        public override void Tile ( ) {
            TilePlacer.PlaceCorridorGroundTiles ( points: GroundPoints.ToArray ( ), outputTilemap: OutputTilemap );
            
            
            TilePlacer.PlaceCorridorWallTiles ( points: WallPoints.ToArray ( ), outputTilemap: OutputTilemap );
        }
    }
}
