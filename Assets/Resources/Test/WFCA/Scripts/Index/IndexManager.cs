using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class IndexManager<T>
{
    private int[][] indicesGrid;
    private Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
    private int currentIndex = 0;

    public IndexManager( IValue<T>[][] valuesGrid ) {
        CreateIndicesGrid( valuesGrid );
    }

    private void CreateIndicesGrid( IValue<T>[][] valuesGrid ) {
        indicesGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(valuesGrid.Length, valuesGrid[ 0 ].Length);

        for( int row = 0; row < indicesGrid.Length; row++ )
            for( int col = 0; col < indicesGrid[ 0 ].Length; col++ )
                SetIndexToGridPosition( valuesGrid, row, col );
    }

    private void SetIndexToGridPosition( IValue<T>[][] valuesGrid, int row, int col ) {
        if( valueIndexDictionary.ContainsValue( valuesGrid[ row ] [ col ] ) ) {
            int key = valueIndexDictionary.FirstOrDefault( x => x.Value.Equals( valuesGrid[ row ][ col ] ) ).Key;
            indicesGrid[ row ][ col ] = key;
        }
        else {
            indicesGrid[ row ][ col ] = currentIndex++;
            valueIndexDictionary.Add( indicesGrid[ row ][ col ], valuesGrid[ row ][ col ] );
        }
    }

    public int GetIndex( IValue<T> value ) {
        if( valueIndexDictionary.ContainsValue( value ) )
            return valueIndexDictionary.FirstOrDefault( x => x.Value.Equals( value ) ).Key;
        else
            throw new System.Exception( "No index for value " + value + " in valueIndexDictionary" );
    }

    public int GetIndex( int row, int col ) {
        if( row < 0 || row >= indicesGrid[ 0 ].Length || col < 0 || col >= indicesGrid.Length )
            throw new System.IndexOutOfRangeException( "indicesGrid doesn't contain a value for row: " + row + ", col: " + col );
        
        return indicesGrid[ col ][ row ];
    }

    public int GetIndexOffseted( int x, int y ) {
        int xMax = indicesGrid[0].Length, yMax = indicesGrid.Length;

        y %= yMax;
        if( y < 0 ) y += yMax;

        x %= xMax;
        if( x < 0 ) x += xMax;

        return indicesGrid[ y ][ x ];
    }

    public int[][] GetPatternIndicesGrid( int col, int row, int patternSize ) {
        int[][] patternIndicesGrid = MyCollectionExtension.CreateJaggedArray<int[][]>( patternSize, patternSize );

        for( int rowOffset = 0; rowOffset < patternSize; rowOffset++ )
            for( int colOffset = 0; colOffset < patternSize; colOffset++ )
                patternIndicesGrid[ rowOffset ][ colOffset ] = GetIndexOffseted( col + colOffset, row + rowOffset );

        return patternIndicesGrid;
    }
    
    public IValue<T> GetValue( int index ) {
        if( valueIndexDictionary.ContainsKey( index ) )
            return valueIndexDictionary[ index ];
        else
            throw new System.Exception( "No value for index " + index + " in valueIndexDictionary" );
    }

    internal Vector2 GetGridSize() {
        if( indicesGrid == null )
            return Vector2.zero;

        return new Vector2( indicesGrid[0].Length, indicesGrid.Length );
    }
}
