using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapOutput : IOutputCreator<Tilemap>
{
    public Tilemap outputImage;
    public Tilemap OutputImage => outputImage;
    public IndexManager<TileBase> indexManager;

    public TilemapOutput( Tilemap outputImage, IndexManager<TileBase> indexManager ) {
        this.outputImage = outputImage;
        this.indexManager = indexManager;
    }

    public void CreateOutput( PatternManager patternManager, int[ ][ ] outputValues, int width, int height ) {
        if( outputValues.Length == 0 )
            return;
        
        outputImage.ClearAllTiles();

        int[ ][ ] valuesGrid = patternManager.ConvertPatternToValues<TileBase>( outputValues );

        // DEBUG START
        // StringBuilder builder = new StringBuilder();
        // foreach( int[ ] row in valuesGrid ) {
        //     builder.Append("[");
        //     foreach( int element in row )
        //         builder.Append(element + ", ");
        //     builder.Append("]\n");
        // }
        // Debug.Log("---Values Grid---");
        // Debug.Log(builder); 
        // Debug.Log("------");
        // DEBUG END

        for( int row = 0; row < valuesGrid.Length; row++ )
            for( int col = 0; col < valuesGrid[ 0 ].Length; col++ ) {
                TileBase tile = ( TileBase )this.indexManager.GetValue( valuesGrid[ row ][ col ] ).Value;
                this.outputImage.SetTile( new Vector3Int( col, row, 0 ), tile );
            }   
    }
}
