using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaxWorldGeneration
{
    public class WaxTilePlacer : MonoBehaviour
    {
        [SerializeField] private TileBase defaultTile;
        [SerializeField] private TileBase color1TopTile;
        [SerializeField] private TileBase color1BottomTile;
        [SerializeField] private TileBase color2TopTile;
        [SerializeField] private TileBase color2BottomTile;
        [SerializeField] private TileBase color3TopTile;
        [SerializeField] private TileBase color3BottomTile;
        [SerializeField] private TileBase[] middleTiles;
        
        public void PlaceFloorTiles ( Vector3Int[] points, Tilemap outputTilemap ) {
            WorldGenHelper.SortVector3IntArray ( points );

             foreach ( Vector3Int point in points )
                outputTilemap.SetTile ( point, defaultTile );

            int vertexCount = points.Length;
            Vector3Int currentDimensions = points[0];
            Vector3Int maxDimensions = points[ vertexCount - 1 ];

            // while ( currentDimensions.x < maxDimensions.x || currentDimensions.y < maxDimensions.y ) {
            //     // for(  ) {

            //     // }
            // }
        }

        public void PlaceWallTiles ( Vector3Int[] points, Tilemap outputTilemap ) {

        }
    }
}
