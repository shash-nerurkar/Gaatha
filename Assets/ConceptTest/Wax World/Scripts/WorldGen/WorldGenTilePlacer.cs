using UnityEngine;
using UnityEngine.Tilemaps;


namespace WorldGeneration
{
    public abstract class WorldGenTilePlacer : MonoBehaviour
    {
        #region Serialized Fields
        
        [ SerializeField ] private TileBase defaultTile;

        #endregion


        #region Fields

        public TileBase DefaultTile {
            get { return defaultTile; }
        }

        #endregion


        #region Methods

        public virtual void PlaceDimensionTiles ( Vector3Int position, Vector3Int dimensions, Tilemap outputTilemap ) {
            Vector3Int bottomLeft = position - dimensions / 2;
            Vector3Int topRight = position + dimensions / 2;

            for ( int i = bottomLeft.x; i <= topRight.x; i++ ) {
                outputTilemap.SetTile ( new Vector3Int ( i, bottomLeft.y, position.z ), defaultTile );
                outputTilemap.SetTile ( new Vector3Int ( i, topRight.y, position.z ), defaultTile );
            } 
            for ( int i = bottomLeft.y; i <= topRight.y; i++ ) {
                outputTilemap.SetTile ( new Vector3Int ( bottomLeft.x, i, position.z ), defaultTile );
                outputTilemap.SetTile ( new Vector3Int ( topRight.x, i, position.z ), defaultTile );
            }  
        }

        public abstract void Init ( WorldGenFloor floor );



        public abstract void PlaceFloorGroundTiles ( Vector3Int [ ] points, Tilemap outputTilemap );

        public abstract void PlaceFloorWallTiles ( Vector3Int [ ] points, Tilemap outputTilemap );
        


        public abstract void PlaceRoomGroundTiles ( Vector3Int [ ] points, Tilemap outputTilemap );

        public abstract void PlaceRoomWallTiles ( Vector3Int[ ] points, Tilemap outputTilemap );
        


        public abstract void PlaceCorridorGroundTiles ( Vector3Int [ ] points, Tilemap outputTilemap );
        
        public abstract void PlaceCorridorWallTiles ( Vector3Int [ ] points, Tilemap outputTilemap );

        #endregion
    }
}
