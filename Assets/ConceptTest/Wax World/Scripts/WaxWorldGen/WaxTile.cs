using UnityEngine;
using UnityEngine.Tilemaps;


namespace WorldGeneration
{
    namespace WaxWorldGeneration
    {
        [ CreateAssetMenu ( fileName = "WaxTile", menuName = "Tiles/Wax Tile" ) ]
        public class WaxTile : ScriptableObject
        {
            [ Header ( "Info" ) ]
            [ SerializeField ] private TileBase tile;
            public TileBase Tile { get { return tile; } }
            [ SerializeField ] private WaxTileLevels level;
            public WaxTileLevels Level { get { return level; } }
            


            [ Header ( "Colors" ) ]
            [ SerializeField ] private WaxTileColors leftColor;
            public WaxTileColors LeftColor { get { return leftColor; } }
            [ SerializeField ] private WaxTileColors middleColor;
            public WaxTileColors MiddleColor { get { return middleColor; } }
            [ SerializeField ] private WaxTileColors rightColor;
            public WaxTileColors RightColor { get { return rightColor; } }



            public WaxTileColors GetColorInPositionOnTile ( WaxColorPositionsOnTile waxColorPositionOnTileToReturn ) {
                switch ( waxColorPositionOnTileToReturn ) {
                    case WaxColorPositionsOnTile.Left:
                        return LeftColor;
                    
                    default:
                    case WaxColorPositionsOnTile.Center:
                        return MiddleColor;

                    case WaxColorPositionsOnTile.Right:
                        return RightColor;
                }
            }

            public bool IsTile ( WaxTileColors [ ] tileColors ) {
                if ( tileColors.Length  <  3 )
                    return false;

                return LeftColor == tileColors [ 0 ] && MiddleColor == tileColors [ 1 ] && RightColor == tileColors [ 2 ];
            }

            public bool IsTile ( TileBase tile ) {
                return tile == Tile;
            }


            public override string ToString ( ) {
                return "Tile: " + Level + "->  " + LeftColor + " | " + MiddleColor + " | " + RightColor;
            }
        }
    }
}