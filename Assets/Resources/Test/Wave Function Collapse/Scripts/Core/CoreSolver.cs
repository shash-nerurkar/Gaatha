using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreSolver
{
    private PatternManager patternManager;
    private OutputGrid outputGrid;
    private CoreHelper coreHelper;
    private PropagationHelper propagationHelper;

    public CoreSolver( PatternManager patternManager, OutputGrid outputGrid ) {
        this.patternManager = patternManager;
        this.outputGrid = outputGrid;
        this.coreHelper = new CoreHelper( patternManager, outputGrid );
        this.propagationHelper = new PropagationHelper( outputGrid, coreHelper );
    }

    public void Propagate() {
        while( propagationHelper.PairsToPropagate.Count > 0 ) {
            VectorPair pairToPropagate = propagationHelper.PairsToPropagate.Dequeue();

            if( propagationHelper.ShouldPairBeProcessed( pairToPropagate ) )
                ProcessCell( pairToPropagate );
            
            if( propagationHelper.IsCollidedCellPresent || outputGrid.IsGridSolved() )
                return;
        }
    }

    private void ProcessCell( VectorPair pairToPropagate ) {
        if( outputGrid.IsCellCollapsed( pairToPropagate.NextCellPosition ) )
            propagationHelper.EnqueueNeighbours( pairToPropagate );
        else
            PropagateNeighbour( pairToPropagate );
    }

    private void PropagateNeighbour( VectorPair pairToPropagate ) {
        HashSet<int> neighbourPossibleValues = outputGrid.GetPossibleValues( pairToPropagate.NextCellPosition );

        int currentPossiblePatternCount = neighbourPossibleValues.Count;

        RemoveImpossibleNeighbours( pairToPropagate, neighbourPossibleValues );

        int newPossiblePatternCount = neighbourPossibleValues.Count;

        propagationHelper.AnalyzePropagationResults( pairToPropagate, currentPossiblePatternCount, newPossiblePatternCount );
    }

    private void RemoveImpossibleNeighbours( VectorPair pairToPropagate, HashSet<int> neighbourPossibleValues ) {
        HashSet<int> possibleIndices = new HashSet<int>();
        
        foreach ( int patternIndexAtBase in outputGrid.GetPossibleValues( pairToPropagate.CellPosition ) ) {
            HashSet<int> possibleNeighboursForBase = patternManager.GetPossibleNeighboursInDirection( patternIndexAtBase, pairToPropagate.DirectionFromBase );
            
            possibleIndices.UnionWith( possibleNeighboursForBase );
        }

        neighbourPossibleValues.IntersectWith( possibleIndices );
    }

    public Vector2Int GetLowestEntropyCell() {
        if( propagationHelper.LowEntropySet.Count <= 0 )
            return outputGrid.GetRandomCell();
        else {
            LowEntropyCell lowestEntropyCell = propagationHelper.LowEntropySet.First();

            propagationHelper.LowEntropySet.Remove( lowestEntropyCell );

            return lowestEntropyCell.Position;
        }
    }

    public void CollapseCell( Vector2Int cellCoordinates ) {
        List<int> possibleValues = outputGrid.GetPossibleValues( cellCoordinates ).ToList();

        if( possibleValues.Count == 0 || possibleValues.Count == 1 )
            return;
        else {
            int index = coreHelper.SelectSolutionPatternByFrequency( possibleValues );

            outputGrid.SetPatternOnPosition( cellCoordinates, possibleValues[ index ] );
        }

        if( coreHelper.HasCollisionOccurred( cellCoordinates ) == false )
            propagationHelper.AddToPropagateQueue( cellCoordinates, cellCoordinates );
        else
            propagationHelper.IsCollidedCellPresent = true;
    }

    public bool IsSolved() => outputGrid.IsGridSolved();

    public bool HasCollisionOccurred() => propagationHelper.IsCollidedCellPresent;
}
