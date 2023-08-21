using System.Collections.Generic;

public class PatternDataResults
{
    private int[][] patternIndicesGrid;
    public Dictionary<int, PatternData> PatternDataIndexDictionary { get; private set; }

    public PatternDataResults( int[][] patternIndicesGrid, Dictionary<int, PatternData> patternIndexDictionary ) {
        this.patternIndicesGrid = patternIndicesGrid;
        PatternDataIndexDictionary = patternIndexDictionary;
    }

    public int GetGridLengthX() => patternIndicesGrid[0].Length;

    public int GetGridLengthY() => patternIndicesGrid.Length;

    public int GetIndex( int row, int col ) => patternIndicesGrid[ col ][ row ];

    public int GetNeighbourInDirection( int row, int col, Direction dir ) {
        if( patternIndicesGrid.CheckJaggedArrayIndex( row, col ) == false )
            return -1;
        
        switch ( dir ) {
            case Direction.Up:
                if( patternIndicesGrid.CheckJaggedArrayIndex( row, col + 1 ) )
                    return GetIndex( row, col + 1 );
                break;

            case Direction.Down:
                if( patternIndicesGrid.CheckJaggedArrayIndex( row, col - 1 ) )
                    return GetIndex( row, col - 1 );
                break;
                
            case Direction.Left:
                if( patternIndicesGrid.CheckJaggedArrayIndex( row - 1, col ) )
                    return GetIndex( row - 1, col );
                break;
                
            case Direction.Right:
                if( patternIndicesGrid.CheckJaggedArrayIndex( row + 1, col ) )
                    return GetIndex( row + 1, col );
                break; 
        }

        return -1;
    }
}
