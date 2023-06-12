using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CoreHelper
{
    private float totalFrequency = 0;
    private float totalFrequencyLog = 0;
    private PatternManager patternManager;
    private OutputGrid outputGrid;

    public CoreHelper( PatternManager patternManager, OutputGrid outputGrid ) {
        this.patternManager = patternManager;
        this.outputGrid = outputGrid;

        // for( int index = 0; index < patternManager.GetPatternsCount(); index++ )
        //     totalFrequency += patternManager.GetPatternFrequency( index );

        // totalFrequencyLog = Mathf.Log( totalFrequency, 2 );
    }

    public int SelectSolutionPatternByFrequency( List<int> patternIndices ) {
        List<float> valueFrequencies = GetWeights( patternIndices );

        float randomValue = UnityEngine.Random.Range( 0, valueFrequencies.Sum() );
        float sum = 0;
        int index = 0;

        foreach( float frequency in valueFrequencies ) {
            sum += frequency;
            
            if( randomValue <= sum )
                return index;
            
            ++index;
        }

        return index;
    }

    private List<float> GetWeights( List<int> patternIndices ) => patternIndices.Select( i => patternManager.GetPatternFrequency( i ) ).ToList();

    public List<VectorPair> GetAllNeighbours( Vector2Int cellCoordinates, Vector2Int previousCell ) => 
        new List<VectorPair>() {
            new VectorPair( cellPosition: cellCoordinates, nextCellPosition: cellCoordinates + new Vector2Int( 1, 0 ), previousCellPosition: previousCell, directionFromBase: Direction.Right ),
            new VectorPair( cellPosition: cellCoordinates, nextCellPosition: cellCoordinates + new Vector2Int( -1, 0 ), previousCellPosition: previousCell, directionFromBase: Direction.Left ),
            new VectorPair( cellPosition: cellCoordinates, nextCellPosition: cellCoordinates + new Vector2Int( 0, 1 ), previousCellPosition: previousCell, directionFromBase: Direction.Up ),
            new VectorPair( cellPosition: cellCoordinates, nextCellPosition: cellCoordinates + new Vector2Int( 0, -1 ), previousCellPosition: previousCell, directionFromBase: Direction.Down )
        };

    public List<VectorPair> GetAllNeighbours( Vector2Int cellCoordinates ) => GetAllNeighbours( cellCoordinates, cellCoordinates );

    public List<VectorPair> GetUncollapsedNeighbours( VectorPair vectorPair ) => 
        GetAllNeighbours( vectorPair.NextCellPosition, vectorPair.CellPosition )
        .Where( x => outputGrid.CheckPositionValidity( x.NextCellPosition ) && outputGrid.IsCellCollapsed( x.NextCellPosition ) == false )
        .ToList();

    public float CalculateEntropy( Vector2Int position ) {
        float sum = 0;

        foreach( int index in outputGrid.GetPossibleValues( position ) ) {
            totalFrequency += patternManager.GetPatternFrequency( index );
            sum += patternManager.GetPatternFrequencyLog2( index );
        }
        
        totalFrequencyLog = Mathf.Log( totalFrequency, 2 );

        return totalFrequencyLog - ( sum / totalFrequency );
    }

    public bool HasCollisionOccurred( Vector2Int cellCoordinates ) {
        foreach ( VectorPair neighbour in GetAllNeighbours( cellCoordinates ) ) {
            if( outputGrid.CheckPositionValidity( neighbour.NextCellPosition ) == false )
                continue;
            
            HashSet<int> neighbourPossibleNeighbourIndices = new HashSet<int>();
            foreach ( int patternIndexAtNeightbour in outputGrid.GetPossibleValues( neighbour.NextCellPosition ) ) {
                HashSet<int> neighboursOfNeighbourInBasePosition = patternManager.GetPossibleNeighboursInDirection( patternIndexAtNeightbour, neighbour.DirectionFromBase.GetOppositeDirection() );
                neighbourPossibleNeighbourIndices.UnionWith( neighboursOfNeighbourInBasePosition );
            }

            if( neighbourPossibleNeighbourIndices.Contains( outputGrid.GetPossibleValues( cellCoordinates ).First() ) == false )
                return true;
        }

        return false;
    }
}
