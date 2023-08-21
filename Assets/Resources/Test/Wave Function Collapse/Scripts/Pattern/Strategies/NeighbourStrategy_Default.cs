using System.Collections.Generic;

public class NeighbourStrategy_Default : IFindNeighbourStrategy
{
    public Dictionary<int, PatternNeighbours> FindNeighbours( PatternDataResults patternDataResults ) {
        Dictionary<int, PatternNeighbours> result = new Dictionary<int, PatternNeighbours>();

        FindNeighboursForEachPattern( patternDataResults, result );

        // DEBUG START
        // foreach( KeyValuePair<int, PatternNeighbours> res in result )
        //     UnityEngine.Debug.Log(res.Key + "->\n" + res.Value.ToString());
        // DEBUG END

        return result;
    }

    private void FindNeighboursForEachPattern( PatternDataResults patternDataResults, Dictionary<int, PatternNeighbours> result ) {
        for( int row = 0; row < patternDataResults.GetGridLengthY(); row++ )
            for( int col = 0; col < patternDataResults.GetGridLengthX(); col++ ) {
                PatternNeighbours neighbours = PatternFinder.GetAllNeighbours( row, col, patternDataResults );

                PatternFinder.AddNeighboursToDictionary( result, patternDataResults.GetIndex( row, col ), neighbours );     
            }
    }
}
