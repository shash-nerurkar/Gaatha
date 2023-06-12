using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputImageParameters
{
    private Tilemap inputTilemap;

    // VARIABLES
    private Vector2Int? bottomLeftCoords;
    private Vector2Int? topRightCoords;
    private BoundsInt inputTilemapBounds;
    private TileBase[] inputTilesArray;
    private Queue<TileContainer> tilesQueue = new Queue<TileContainer>();
    private int width = 0, height = 0;

    public Queue<TileContainer> TilesQueue { get => tilesQueue; set => tilesQueue = value; }
    public int Width { get => width; }
    public int Height { get => height; }

    public InputImageParameters( Tilemap inputTilemap ) {
        this.inputTilemap = inputTilemap;
        
        inputTilemapBounds = inputTilemap.cellBounds;
        inputTilesArray = inputTilemap.GetTilesBlock( bounds: inputTilemapBounds );
        
        ExtractNonEmptyTiles();
        VerifyInputTiles();
    }

    private void ExtractNonEmptyTiles() {
        for( int row = 0; row < inputTilemapBounds.size.y; row++ )
            for( int col = 0; col < inputTilemapBounds.size.x; col++ ){
                int index = col + ( row * inputTilemapBounds.size.x );

                TileBase tile = inputTilesArray[ index ];
                if( tile != null ) {
                    if( bottomLeftCoords == null )
                        bottomLeftCoords = new Vector2Int( col, row );
                    
                    tilesQueue.Enqueue( new TileContainer( tile, col, row ) );

                    topRightCoords = new Vector2Int( col, row );
                }                
            }
    }

    private void VerifyInputTiles() {
        if( topRightCoords == null || bottomLeftCoords == null )
            throw new Exception("WFC: Input Tilemap is empty!");

        width = Mathf.Abs( topRightCoords.Value.x - bottomLeftCoords.Value.x ) + 1;
        height = Mathf.Abs( topRightCoords.Value.y - bottomLeftCoords.Value.y ) + 1;

        if( tilesQueue.Count != ( width * height ) ) 
            throw new Exception("WFC: Input Tilemap is not a complete square grid! Reason: Missing tiles inside square.");
        
        if( tilesQueue.Any( Tile => Tile.X > topRightCoords.Value.x || Tile.X < bottomLeftCoords.Value.x || Tile.Y > topRightCoords.Value.y || Tile.Y < bottomLeftCoords.Value.y ) )
            throw new Exception("WFC: Input Tilemap is not a complete square grid! Reason: Extra tiles outside square.");
    }
}
