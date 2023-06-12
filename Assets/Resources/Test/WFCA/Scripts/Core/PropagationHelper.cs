using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropagationHelper
{
    private OutputGrid outputGrid;
    private CoreHelper coreHelper;
    public bool IsCollidedCellPresent;
    private SortedSet<LowEntropyCell> lowEntropySet = new SortedSet<LowEntropyCell>();
    private Queue<VectorPair> pairsToPropagate = new Queue<VectorPair>();

    public SortedSet<LowEntropyCell> LowEntropySet { get => lowEntropySet; }
    public Queue<VectorPair> PairsToPropagate { get => pairsToPropagate; }

    public PropagationHelper( OutputGrid outputGrid, CoreHelper coreHelper ) {
        this.outputGrid = outputGrid;
        this.coreHelper = coreHelper; 
    }

    public bool ShouldPairBeProcessed( VectorPair pairToPropagate ) => outputGrid.CheckPositionValidity( pairToPropagate.NextCellPosition ) && pairToPropagate.AreNextAndPreviousCellsSame() == false;
    
    public void AnalyzePropagationResults( VectorPair pairToPropagate, int currentPossiblePatternCount, int newPossiblePatternCount ) {
        if( newPossiblePatternCount > 1 && currentPossiblePatternCount > newPossiblePatternCount ) {
            AddToPropagateQueue( pairToPropagate.NextCellPosition, pairToPropagate.CellPosition );
            AddToLowEntropySet( pairToPropagate.NextCellPosition );
        }
        else if( newPossiblePatternCount == 1 )
            IsCollidedCellPresent = coreHelper.HasCollisionOccurred( pairToPropagate.NextCellPosition );
        else if( newPossiblePatternCount == 0 ) {
            IsCollidedCellPresent = true;
            Debug.Log("Cell with no solution after base propagation.");
        }
    }

    public void AddToLowEntropySet( Vector2Int nextCellPosition ) {
        LowEntropyCell lowEntropyCell = lowEntropySet.Where( cell => cell.Position == nextCellPosition ).FirstOrDefault();

        if( lowEntropyCell == null ) {
            float entropy = coreHelper.CalculateEntropy( nextCellPosition );

            lowEntropySet.Add( new LowEntropyCell( nextCellPosition, entropy ) );
        }
        else {
            lowEntropySet.Remove( lowEntropyCell );

            lowEntropyCell.Entropy = coreHelper.CalculateEntropy( nextCellPosition );

            lowEntropySet.Add( lowEntropyCell );
        }
    }

    public void AddToPropagateQueue( Vector2Int nextCellPosition, Vector2Int cellPosition ) {
        List<VectorPair> nextCellNeighbours = coreHelper.GetAllNeighbours( nextCellPosition, cellPosition );

        foreach ( VectorPair neighbour in nextCellNeighbours )
            pairsToPropagate.Enqueue( neighbour );
    }

    internal void EnqueueNeighbours( VectorPair pairToPropagate ) {
        List<VectorPair> uncollapsedNeighbours = coreHelper.GetUncollapsedNeighbours( pairToPropagate );
        
        foreach( VectorPair neighbour in uncollapsedNeighbours )
            pairsToPropagate.Enqueue( neighbour );
    }
}
