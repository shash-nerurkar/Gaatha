using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaxWorldGeneration
{
    public class WaxTilePlacer : MonoBehaviour
    {
        [SerializeField] private TileBase defaultTile;
        
        public void PlaceFloorTiles ( Vector3Int[] points, Tilemap outputTilemap ) {
            WorldGenHelper.SortVector3IntArray ( points );

             foreach ( Vector3Int point in points )
                outputTilemap.SetTile ( point, defaultTile );
        }

        public void PlaceWallTiles ( Vector3Int[] points, Tilemap outputTilemap ) {

        }
    }
}
