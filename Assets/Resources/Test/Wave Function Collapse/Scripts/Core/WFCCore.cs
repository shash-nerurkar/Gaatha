using UnityEngine;

public class WFCCore
{
    public OutputGrid OutputGrid { get; }
    private PatternManager patternManager;

    private int maxIterations = 0;

    private int maxInnerIterations = 0;

    public WFCCore( int outputWidth, int outputHeight, int maxIterations, int maxInnerIterations, PatternManager patternManager ) {
        this.OutputGrid = new OutputGrid( outputWidth, outputHeight, patternManager.GetPatternsCount() );
        this.patternManager = patternManager;
        this.maxIterations = maxIterations;
        this.maxInnerIterations = maxInnerIterations;
    }

    public int[][] CreateOutputGrid() {
        int iteration = 0;
        
        while( iteration < this.maxIterations ) {
            CoreSolver solver = new CoreSolver( patternManager, OutputGrid );
            int innerIterationsCount = maxInnerIterations;

            while( !solver.HasCollisionOccurred() && !solver.IsSolved() ) {
                Vector2Int position = solver.GetLowestEntropyCell();

                solver.CollapseCell( position );
                
                solver.Propagate();

                --innerIterationsCount;
                
                if( innerIterationsCount <= 0 ) {
                    Debug.Log( "WFC: Propagation is taking too long!" );
                    return new int[ 0 ][  ];
                }
            }

            if( solver.HasCollisionOccurred() ) {
                Debug.Log( "\n Conflict occured. Iteration: " + iteration );
                
                ++iteration;

                OutputGrid.ResetAllPossibilities();

                solver = new CoreSolver( patternManager, OutputGrid );
            }
            else {
                Debug.Log( "Solved on: " + iteration );
                
                break;
            }
        }

        if( iteration >= maxIterations )
            Debug.Log( "Couldn't solve tilemap." );

        return OutputGrid.GetSolvedOutputGrid();
    }
}
