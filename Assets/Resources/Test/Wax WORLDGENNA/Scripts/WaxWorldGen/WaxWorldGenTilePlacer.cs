using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


namespace WorldGeneration
{
    namespace WaxWorldGeneration
    {
        [ Serializable ] public enum WaxTileColors {
            Color1,
            Color2,
            Color3,
            Count,
            None
        }

        [ Serializable ] public enum WaxTileLevels {
            Top,
            Bottom,
            Middle,
            Default
        }

        [ Serializable ] public enum WaxColorPositionsOnTile {
            Left,
            Center,
            Right
        }

        public class WaxWorldGenTilePlacer : WorldGenTilePlacer
        {
            [ SerializeField ] private WaxTile [ ] defaultTiles;
            [ SerializeField ] private WaxTile [ ] topTiles;
            [ SerializeField ] private WaxTile [ ] bottomTiles;
            [ SerializeField ] private WaxTile [ ] middleTiles;

            private Tilemap outputTilemap;
            

            public override void PlaceFloorTiles ( Vector3Int [ ] points, Tilemap outputTilemap ) {
                this.outputTilemap = outputTilemap;

                WorldGenHelper.SortVector3IntArray ( points );
                int zValue = points[ 0 ].z;

                int vertexCount = points.Length;
                Vector2Int minDimensions = ( Vector2Int ) WorldGenHelper.GetMinPositionValues ( points );
                Vector2Int maxDimensions = ( Vector2Int ) WorldGenHelper.GetMaxPositionValues ( points );
                Vector2Int dimensionRange = maxDimensions - minDimensions;

                Vector2Int currentStartDimensions = minDimensions;
                Vector2Int currentEndDimensions = minDimensions;

                while ( currentEndDimensions.x < maxDimensions.x ) {
                    currentStartDimensions.x = currentEndDimensions.x;

                    int positionToFillUntilX = Random.Range ( Mathf.FloorToInt ( dimensionRange.x / 5 ), Mathf.CeilToInt ( dimensionRange.x / 3 ) );
                    positionToFillUntilX = positionToFillUntilX < 1 ? 1 : positionToFillUntilX;
                    
                    currentEndDimensions.x += positionToFillUntilX;

                    while ( currentEndDimensions.y  <  maxDimensions.y ) {
                        int positionToFillUntilY = Random.Range ( Mathf.FloorToInt ( dimensionRange.y / 5 ), Mathf.CeilToInt ( dimensionRange.y / 3 ) );
                        positionToFillUntilY = positionToFillUntilY < 1 ? 1 : positionToFillUntilY;
                        
                        currentEndDimensions.y += positionToFillUntilY;

                        WaxTileColors color = GetRandomTileColor ( );

                        for ( int row = currentStartDimensions.x; row  <= currentEndDimensions.x; row++ )
                            for ( int col = currentStartDimensions.y; col  <= currentEndDimensions.y; col++ ) {
                                Vector3Int currentPoint = new Vector3Int ( row, col, zValue );

                                if ( WorldGenHelper.ArrayContains ( points, currentPoint ) )
                                    SetFloorTile ( currentPoint, color, forceOverride: false );
                            }

                        currentStartDimensions.y = currentEndDimensions.y;
                    }

                    currentStartDimensions.y = minDimensions.y;

                    currentEndDimensions.y = minDimensions.y;
                }
            }

            private void SetFloorTile ( Vector3Int position, WaxTileColors color, bool forceOverride = true ) {

                WaxTile GetWaxTileAtLevel ( WaxTileLevels level ) {
                    
                    WaxTileColors FetchAdjacentTileColor ( Vector3Int offset, WaxTile [ ] tilesToCheck, WaxColorPositionsOnTile positionOnTile ) {
                        TileBase tile = outputTilemap.GetTile ( position + offset );

                        if ( tile != null )
                            foreach ( WaxTile waxTile in tilesToCheck )
                                if ( waxTile.Tile == tile )
                                    return waxTile.GetColorInPositionOnTile ( positionOnTile );
                        
                        return WaxTileColors.None;
                    }


                    WaxTileColors [ ] tileColors = new WaxTileColors [ 3 ];

                    switch ( level ) {
                        case WaxTileLevels.Top:
                            WaxTileColors c;
                            
                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, 1 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                            if ( c != WaxTileColors.None )
                                tileColors [ 0 ] = c;
                            else {
                                c = FetchAdjacentTileColor ( offset: new Vector3Int ( -1, 1 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                                if ( c != WaxTileColors.None )
                                    tileColors [ 0 ] = c;
                                else {
                                    c = FetchAdjacentTileColor ( offset: new Vector3Int ( -1, 0 ), tilesToCheck: topTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                                    if ( c != WaxTileColors.None )
                                        tileColors [ 0 ] = c;
                                    else
                                        tileColors [ 0 ] = GetRandomTileColor ( );
                                }
                            }

                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, -1 ), tilesToCheck: bottomTiles, positionOnTile: WaxColorPositionsOnTile.Center );
                            if ( c != WaxTileColors.None )
                                tileColors [ 1 ] = c;
                            else
                                tileColors [ 1 ] = color;

                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, 1 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                            if ( c != WaxTileColors.None )
                                tileColors [ 2 ] = c;
                            else {
                                c = FetchAdjacentTileColor ( offset: new Vector3Int ( 1, 1 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                                if ( c != WaxTileColors.None )
                                    tileColors [ 2 ] = c;
                                else {
                                    c = FetchAdjacentTileColor ( offset: new Vector3Int ( 1, 0 ), tilesToCheck: topTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                                    if ( c != WaxTileColors.None )
                                        tileColors [ 2 ] = c;
                                    else
                                        tileColors [ 2 ] = GetRandomTileColor ( );
                                }
                            }


                            foreach ( WaxTile waxTile in topTiles )
                                if ( waxTile.IsTile ( tileColors: tileColors ) )
                                    return waxTile;
                            break;



                        case WaxTileLevels.Bottom:
                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( -1, 0 ), tilesToCheck: bottomTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                            if ( c != WaxTileColors.None )
                                tileColors [ 0 ] = c;
                            else {
                                c = FetchAdjacentTileColor ( offset: new Vector3Int ( -1, -1 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                                if ( c != WaxTileColors.None )
                                    tileColors [ 0 ] = c;
                                else {
                                    c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, -1 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                                    if ( c != WaxTileColors.None )
                                        tileColors [ 0 ] = c;
                                    else
                                        tileColors [ 0 ] = GetRandomTileColor ( );
                                }
                            }
                            
                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, 1 ), tilesToCheck: topTiles, positionOnTile: WaxColorPositionsOnTile.Center );
                            if ( c != WaxTileColors.None )
                                tileColors [ 1 ] = c;
                            else
                                tileColors [ 1 ] = color;
                
                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 1, 0 ), tilesToCheck: bottomTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                            if ( c != WaxTileColors.None )
                                tileColors [ 2 ] = c;
                            else {
                                c = FetchAdjacentTileColor ( offset: new Vector3Int ( 1, -1 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                                if ( c != WaxTileColors.None )
                                    tileColors [ 2 ] = c;
                                else {
                                    c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, -1 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Right);
                                    if ( c != WaxTileColors.None )
                                        tileColors [ 2 ] = c;
                                    else
                                        tileColors [ 2 ] = GetRandomTileColor ( );
                                }
                            }


                            foreach ( WaxTile waxTile in bottomTiles )
                                if ( waxTile.IsTile ( tileColors: tileColors ) )
                                    return waxTile;
                            break;



                        case WaxTileLevels.Middle:
                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, 1 ), tilesToCheck: bottomTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                            if ( c != WaxTileColors.None )
                                tileColors [ 0 ] = c;
                            else {
                                c = FetchAdjacentTileColor ( offset: new Vector3Int ( -1, 1 ), tilesToCheck: bottomTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                                if ( c != WaxTileColors.None )
                                    tileColors [ 0 ] = c;
                                else {
                                    c = FetchAdjacentTileColor ( offset: new Vector3Int ( -1, 0 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                                    if ( c != WaxTileColors.None )
                                        tileColors [ 0 ] = c;
                                    else {
                                        c = FetchAdjacentTileColor ( offset: new Vector3Int ( -1, -1 ), tilesToCheck: topTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                                        if ( c != WaxTileColors.None )
                                            tileColors [ 0 ] = c;
                                        else {
                                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, -1 ), tilesToCheck: topTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                                            if ( c != WaxTileColors.None )
                                                tileColors [ 0 ] = c;
                                            else
                                                tileColors [ 0 ] = GetRandomTileColor ( );
                                        }
                                    }
                                }
                            }
                            
                            tileColors [ 1 ] = WaxTileColors.None;

                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, 1 ), tilesToCheck: bottomTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                            if ( c != WaxTileColors.None )
                                tileColors [ 2 ] = c;
                            else {
                                c = FetchAdjacentTileColor ( offset: new Vector3Int ( 1, 1 ), tilesToCheck: bottomTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                                if ( c != WaxTileColors.None )
                                    tileColors [ 2 ] = c;
                                else {
                                    c = FetchAdjacentTileColor ( offset: new Vector3Int ( 1, 0 ), tilesToCheck: middleTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                                    if ( c != WaxTileColors.None )
                                        tileColors [ 2 ] = c;
                                    else {
                                        c = FetchAdjacentTileColor ( offset: new Vector3Int ( 1, -1 ), tilesToCheck: topTiles, positionOnTile: WaxColorPositionsOnTile.Left );
                                        if ( c != WaxTileColors.None )
                                            tileColors [ 2 ] = c;
                                        else {
                                            c = FetchAdjacentTileColor ( offset: new Vector3Int ( 0, -1 ), tilesToCheck: topTiles, positionOnTile: WaxColorPositionsOnTile.Right );
                                            if ( c != WaxTileColors.None )
                                                tileColors [ 2 ] = c;
                                            else
                                                tileColors [ 2 ] = GetRandomTileColor ( );
                                        }
                                    }
                                }
                            }


                            foreach ( WaxTile waxTile in middleTiles )
                                if ( waxTile.IsTile ( tileColors: tileColors ) )
                                    return waxTile;
                            break;
                    }

                    return defaultTiles [ 0 ];
                }


                if ( forceOverride || !outputTilemap.HasTile ( position ) ) {
                    WaxTile waxTile;

                    switch ( Mathf.Abs ( position.y ) % 3 ) {
                        case 0:
                            waxTile = GetWaxTileAtLevel ( level: WaxTileLevels.Top );
                            break;
                        
                        case 1:
                            if ( Mathf.Sign ( position.y ) > 0 )
                                waxTile = GetWaxTileAtLevel ( level: WaxTileLevels.Middle );
                            else
                                waxTile = GetWaxTileAtLevel ( level: WaxTileLevels.Bottom );
                            break;
                        
                        case 2:
                            if ( Mathf.Sign ( position.y ) > 0 )
                                waxTile = GetWaxTileAtLevel ( level: WaxTileLevels.Bottom );
                            else
                                waxTile = GetWaxTileAtLevel ( level: WaxTileLevels.Middle );
                            break;
                        
                        default:
                            return;
                    }

                    outputTilemap.SetTile ( position, waxTile.Tile );
                }
            }

            private WaxTileColors GetRandomTileColor ( ) => ( WaxTileColors ) Random.Range ( 0, ( int ) WaxTileColors.Count );



            public override void PlaceWallTiles ( Vector3Int[ ] points, Tilemap outputTilemap ) { }
        }
    }
}

