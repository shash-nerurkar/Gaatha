using UnityEngine;
using UnityEngine.Tilemaps;


namespace WorldGeneration
{
    public abstract class WorldGenTilePlacer : MonoBehaviour
    {
        [ SerializeField ] private TileBase defaultTile;
        
        public abstract void PlaceFloorTiles ( Vector3Int [ ] points, Tilemap outputTilemap );

        public abstract void PlaceWallTiles ( Vector3Int[ ] points, Tilemap outputTilemap );
    }
}
