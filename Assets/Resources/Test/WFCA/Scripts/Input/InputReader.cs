using System;
using UnityEngine.Tilemaps;

public class InputReader : IInputReader<TileBase>
{
    public InputReader( Tilemap inputTilemap ) {
        this.inputTilemap = inputTilemap;
    }

    // COMPONENTS
    private Tilemap inputTilemap;

    public IValue<TileBase>[][] ReadInputToGrid() {
        TileBase[][] grid = ReadInputTilemap();

        TileBaseValue[][] valuesGrid = null;
        if( grid != null ) {
            valuesGrid = MyCollectionExtension.CreateJaggedArray<TileBaseValue[][]>( grid.Length, grid[ 0 ].Length );

            for( int row = 0; row < grid.Length; row++ )
                for( int col = 0; col < grid[0].Length; col++ )
                    valuesGrid[ row ][ col ] = new TileBaseValue( grid[ row ][ col ] );
        }

        return valuesGrid;
    }

    private TileBase[][] ReadInputTilemap() {
        InputImageParameters imageParameters = new InputImageParameters( inputTilemap );

        return CreateTileBaseGrid( imageParameters );
    }

    private TileBase[][] CreateTileBaseGrid( InputImageParameters imageParameters ) {
        TileBase[][] inputTilesGrid = MyCollectionExtension.CreateJaggedArray<TileBase[][]>( imageParameters.Height, imageParameters.Width );
        
        for( int row = 0; row < imageParameters.Height; row++ )
            for( int col = 0; col < imageParameters.Width; col++ )
                inputTilesGrid[ row ][ col ] = imageParameters.TilesQueue.Dequeue().Tile;

        return inputTilesGrid;
    }
}
