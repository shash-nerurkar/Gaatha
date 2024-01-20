using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PatternManager
{
    Dictionary<int, PatternData> patternDataIndexDictionary;
    Dictionary<int, PatternNeighbours> patternPossibleNeighboursDictionary;
    int patternSize = -1;

    IFindNeighbourStrategy strategy;

    public void ProcessGrid<T>( IndexManager<T> indexManager, bool equalWeights, int patternSize, string strategyName ) {
        NeighbourStrategyFactory strategyFactory = new NeighbourStrategyFactory();
        
        this.patternSize = patternSize;

        strategy = strategyFactory.CreateInstance( strategyName == null ? "default" : strategyName );

        CreatePatterns( indexManager, strategy, equalWeights );
    }

    private void CreatePatterns<T>( IndexManager<T> indexManager, IFindNeighbourStrategy strategy, bool equalWeights ) {
        PatternDataResults patternDataResults = PatternFinder.GetPatternDataFromGrid( indexManager, patternSize, equalWeights );

        // DEBUG CODE -> START
        // StringBuilder builder = null;
        // List<string> list = new List<string>();
        // for ( int row = 0; row < patternDataResults.GetGridLengthY(); row++ ) {
        //     builder = new StringBuilder();

        //     for ( int col = 0; col < patternDataResults.GetGridLengthX(); col++ )
        //         builder.Append( patternDataResults.GetIndex( col, row ) + " " );
            
        //     list.Add( builder.ToString() );
        // }
        // list.Reverse();
        // foreach( string str in list )
        //         Debug.Log( str );
        // DEBUG CODE -> END

        patternDataIndexDictionary = patternDataResults.PatternDataIndexDictionary;

        GetPatternNeighbours( patternDataResults, strategy );
    }

    internal int[ ][ ] ConvertPatternToValues<T>( int[ ][ ] outputValues ) {
        int patternOutputWidth = outputValues[0].Length, patternOutputHeight = outputValues.Length;
        int valuesGridWidth = patternOutputWidth + patternSize - 1, valuesGridHeight = patternOutputHeight + patternSize - 1;
        int[ ][ ] valuesGrid = MyCollectionExtension.CreateJaggedArray<int[ ][ ]>( valuesGridHeight, valuesGridWidth );

        for( int row = 0; row < patternOutputHeight; row++ )
            for( int col = 0; col < patternOutputWidth; col++ )
                GetPatternValues( patternOutputWidth, patternOutputHeight, valuesGrid, row, col, GetPatternData( outputValues[ row ][ col ] ).Pattern );

        return valuesGrid;
    }

    private void GetPatternValues( int patternOutputWidth, int patternOutputHeight, int[][] valuesGrid, int row, int col, Pattern pattern ) {
        if( row == patternOutputHeight - 1 && col == patternOutputWidth - 1 ) {
            for( int row_1 = 0; row_1 < patternSize; row_1++ )
                for( int col_1 = 0; col_1 < patternSize; col_1++ )
                    valuesGrid[ row + row_1 ][ col + col_1 ] = pattern.GetGridValue( col_1, row_1 );
        }
        else if( row == patternOutputHeight - 1 ) {
            for( int row_1 = 0; row_1 < patternSize; row_1++ )
                valuesGrid[ row + row_1 ][ col ] = pattern.GetGridValue( 0, row_1 );
        }
        else if( col == patternOutputWidth - 1 ) {
            for( int col_1 = 0; col_1 < patternSize; col_1++ )
                valuesGrid[ row ][ col + col_1 ] = pattern.GetGridValue( col_1, 0 );
        }
        else {
            valuesGrid[ row ][ col ] = pattern.GetGridValue( 0, 0 );
        }
    }

    private void GetPatternNeighbours( PatternDataResults patternDataResults, IFindNeighbourStrategy strategy ) => patternPossibleNeighboursDictionary = PatternFinder.FindPossibleNeighbours( strategy, patternDataResults );

    public PatternData GetPatternData( int patternIndex ) => patternDataIndexDictionary[ patternIndex ];

    public HashSet<int> GetPossibleNeighboursInDirection( int patternIndex, Direction dir ) => patternPossibleNeighboursDictionary[ patternIndex ].GetNeighboursInDirection( dir );

    public float GetPatternFrequency( int patternIndex ) => GetPatternData( patternIndex ).FrequencyRelative;

    public float GetPatternFrequencyLog2( int patternIndex ) => GetPatternData( patternIndex ).FrequencyRelativeLog2;

    public int GetPatternsCount() => patternDataIndexDictionary.Count;
}
