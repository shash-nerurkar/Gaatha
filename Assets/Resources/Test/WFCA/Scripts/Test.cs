using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    // TILEMAPS
    [SerializeField] private Tilemap inputTilemap;
    
    private IndexManager<TileBase> indexManager;
    
    [Header("Pattern")]
    [SerializeField] private bool equalWeights = false;
    [SerializeField] private int patternSize = 2;
    private PatternManager patternManager;

    [Header("Core")]
    [SerializeField] private int outputWidth = 5;
    [SerializeField] private int outputHeight = 5;
    [SerializeField] private int maxIterations = 500;
    [SerializeField] private int maxInnerIterations = 30;
    private WFCCore wfcCore;

    [Header("Output")]
    [SerializeField] private Tilemap outputTilemap;
    [SerializeField] private TilemapOutput output;


    public void CreateWFC() {
        InputReader reader = new InputReader( inputTilemap );
        IValue<TileBase>[][] grid = reader.ReadInputToGrid();

        indexManager = new IndexManager<TileBase>( grid );

        patternManager = new PatternManager();
        patternManager.ProcessGrid<TileBase>(
            indexManager,
            equalWeights: equalWeights,
            patternSize: patternSize,
            strategyName: patternSize > 1 ? "overlay" : "default" 
        );

        wfcCore = new WFCCore(
            outputWidth: outputWidth,
            outputHeight: outputHeight,
            maxIterations: maxIterations,
            maxInnerIterations: maxInnerIterations,
            patternManager: patternManager
        );

        // DEBUG START
        // for ( int row = 0; row < grid.Length; row++ )
        //     for ( int col = 0; col < grid[0].Length; col++ )
        //         Debug.Log( row + ", " + col + ": tile- " + grid[ row ][ col ].Value.name );
        
        // StringBuilder builder = null;
        // List<string> list = new List<string>();
        // for ( int row = -1; row <= grid.Length; row++ ) {
        //     builder = new StringBuilder();

        //     for ( int col = -1; col <= grid[0].Length; col++ )
        //         builder.Append( indexManager.GetIndexOffseted( col, row ) + " " );
            
        //     list.Add( builder.ToString() );
        // }
        // list.Reverse();
        // foreach( string str in list )
        //     Debug.Log( str );

        // foreach( Direction dir in Enum.GetValues( typeof( Direction ) ) )
        //     Debug.Log( dir.ToString() + ": " + string.Join( " ", patternManager.GetPossibleNeighboursInDirection( 0, dir ).ToArray() ) );
        // DEBUG END
    }

    public void CreateTilemap() {
        output = new TilemapOutput( indexManager: indexManager, outputImage: outputTilemap );

        int[ ][ ] result = wfcCore.CreateOutputGrid();
                
        wfcCore.OutputGrid.PrintResultsToConsole();

        // DEBUG START
        StringBuilder builder = new StringBuilder();
        foreach( int[ ] row in result ) {
            builder.Append("[");
            foreach( int element in row )
                builder.Append(element + ", ");
            builder.Append("]\n");
        }
        Debug.Log("---Results tilemap---");
        Debug.Log(builder); 
        Debug.Log("------");
        // DEBUG END

        output.CreateOutput( patternManager: patternManager, outputValues: result, width: outputWidth, height: outputHeight );
    }

    public void SaveTilemap() {
        if( output.outputImage != null ) {
            outputTilemap = output.OutputImage;

            // GameObject objectToSave = outputTilemap.gameObject;
            // PrefabUtility.SaveAsPrefabAsset( objectToSave, "Assets/Saved/output.prefab" );
        }
    }
}