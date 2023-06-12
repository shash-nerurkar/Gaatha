using System;
using System.Collections.Generic;
using UnityEngine;

public static class PatternFinder
{
    internal static PatternDataResults GetPatternDataFromGrid<T>( IndexManager<T> indexManager, int patternSize, bool equalWeights ) {
        Dictionary<string, PatternData> patternHashCodeDictionary = new Dictionary<string, PatternData>();
        Dictionary<int, PatternData> patternIndexDictionary = new Dictionary<int, PatternData>();
        
        Vector2 gridSize = indexManager.GetGridSize();
        int patternGridSizeCol = 0, patternGridSizeRow = 0;

        int rowMin, rowMax, colMin, colMax;

        if( patternSize < 3 ) {
            patternGridSizeCol = ( int )gridSize.x + 3 - patternSize;
            patternGridSizeRow = ( int )gridSize.y + 3 - patternSize;
            colMin = -1;
            colMax = patternGridSizeCol - 1;
            rowMin = -1;
            rowMax = patternGridSizeRow - 1;
        }
        else {
            patternGridSizeCol = ( int )gridSize.x + patternSize - 1;
            patternGridSizeRow = ( int )gridSize.y + patternSize - 1;
            colMin = 1 - patternSize;
            colMax = ( int )gridSize.x;
            rowMin = 1 - patternSize;
            rowMax = ( int )gridSize.y;
        }

        int[][] patternIndicesGrid = MyCollectionExtension.CreateJaggedArray<int[][]>( patternGridSizeRow, patternGridSizeCol );
        int totalFrequency = 0, patternIndex = 0;

        for( int row = rowMin; row < rowMax; row++ )
            for( int col = colMin; col < colMax; col++) {
                int[][] gridValues = indexManager.GetPatternIndicesGrid( col, row, patternSize );
                string hashValue = HashCodeCalculator.CalculateHashCode( gridValues );

                if( patternHashCodeDictionary.ContainsKey( hashValue ) == false ) {
                    Pattern newPattern = new Pattern( gridValues, hashValue, patternIndex );
                    ++patternIndex;

                    AddNewPattern( patternHashCodeDictionary, patternIndexDictionary, hashValue, newPattern );
                }
                else {
                    if( equalWeights == false )
                        patternIndexDictionary[patternHashCodeDictionary[ hashValue ].Pattern.Index].IncreaseFrequency();
                }
                
                ++totalFrequency;
                
                if( patternSize < 3 )
                    patternIndicesGrid[ row + 1 ][ col + 1 ] = patternHashCodeDictionary[ hashValue ].Pattern.Index;
                else
                    patternIndicesGrid[ row + (patternSize - 1) ][ col + (patternSize - 1) ] = patternHashCodeDictionary[ hashValue ].Pattern.Index;
            }

        CalculateRelativeFrequencies( patternIndexDictionary, totalFrequency );

        return new PatternDataResults( patternIndicesGrid, patternIndexDictionary );
    }

    private static void CalculateRelativeFrequencies( Dictionary<int, PatternData> patternIndexDictionary, int totalFrequency ) {
        foreach( PatternData patternData in patternIndexDictionary.Values )
            patternData.CalculateRelativeFrequency( totalFrequency );
    }

    private static void AddNewPattern( Dictionary<string, PatternData> patternHashCodeDictionary, Dictionary<int, PatternData> patternIndexDictionary, string hashValue, Pattern newPattern ) {
        PatternData patternData = new PatternData( newPattern );

        patternIndexDictionary.Add( newPattern.Index, patternData );
        patternHashCodeDictionary.Add( hashValue, patternData );
    }

    internal static Dictionary<int, PatternNeighbours> FindPossibleNeighbours( IFindNeighbourStrategy strategy, PatternDataResults patternDataResults ) {
        return strategy.FindNeighbours( patternDataResults );
    }

    public static PatternNeighbours GetAllNeighbours( int row, int col, PatternDataResults patternDataResults ) {
        PatternNeighbours neighbours = new PatternNeighbours();

        foreach ( Direction dir in Enum.GetValues( typeof( Direction ) ) ) {
            int patternIndex = patternDataResults.GetNeighbourInDirection( row, col, dir );
            
            if( patternIndex >= 0 )
                neighbours.AddPattern( dir, patternIndex );
        }

        return neighbours;
    }

    public static void AddNeighboursToDictionary( Dictionary<int, PatternNeighbours> dictionary, int patternIndex, PatternNeighbours neighbours ) {
        if( dictionary.ContainsKey( patternIndex ) == false )
            dictionary.Add( patternIndex, neighbours );
        
        dictionary[ patternIndex ].AddNeighbour( neighbours );
    }
}
