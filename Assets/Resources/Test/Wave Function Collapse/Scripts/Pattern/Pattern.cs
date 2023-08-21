public class Pattern
{
    private int index;
    private int[][] grid;

    public string HashIndex { get; set; }
    public int Index { get => index; set => index = value; }

    public Pattern( int[][] grid, string hashCode, int index ) {
        this.grid = grid;
        HashIndex = hashCode;
        this.index = index;
    }

    public void SetGridValue( int row, int col, int value ) => grid[ col ][ row ] = value;
    public int GetGridValue( int row, int col ) => grid[ col ][ row ];
    public bool CheckGridValue( int row, int col, int value ) => value.Equals( GetGridValue( row, col ) );

    internal bool ComparePattern( Direction dir, Pattern pattern ) {
        int[][] myGrid = GetGridValuesInDirection( dir );
        int[][] otherGrid = pattern.GetGridValuesInDirection( dir.GetOppositeDirection() );

        for( int row = 0; row < myGrid.Length; row++ )
            for( int col = 0; col < myGrid[0].Length; col++ )
                if( myGrid[ row ][ col ] != otherGrid[ row ][ col ] )
                    return false;
        
        return true;
    }

    private int[][] GetGridValuesInDirection( Direction dir ) {
        int[][] selectedGridValues = null;

        switch ( dir ) {
            case Direction.Up:
                selectedGridValues = MyCollectionExtension.CreateJaggedArray<int[][]>( grid.Length - 1, grid.Length );
                SelectGridValues( 0, grid.Length, 1, grid.Length, selectedGridValues );
                break;
            
            case Direction.Down:
                selectedGridValues = MyCollectionExtension.CreateJaggedArray<int[][]>( grid.Length - 1, grid.Length );
                SelectGridValues( 0, grid.Length, 0, grid.Length - 1, selectedGridValues );
                break;
            
            case Direction.Left:
                selectedGridValues = MyCollectionExtension.CreateJaggedArray<int[][]>( grid.Length, grid.Length - 1 );
                SelectGridValues( 0, grid.Length - 1, 0, grid.Length, selectedGridValues );
                break;

            case Direction.Right:
                selectedGridValues = MyCollectionExtension.CreateJaggedArray<int[][]>( grid.Length, grid.Length - 1 );
                SelectGridValues( 1, grid.Length, 0, grid.Length, selectedGridValues );
                break;
        }

        return selectedGridValues;
    }

    private void SelectGridValues( int colMin, int colMax, int rowMin, int rowMax, int[][] selectedGridValues ) {
        for( int row = rowMin; row < rowMax; row++ ) 
            for( int col = colMin; col < colMax; col++ )
                selectedGridValues[ row - rowMin ][ col - colMin ] = grid[ row ][ col ];
    }
}
