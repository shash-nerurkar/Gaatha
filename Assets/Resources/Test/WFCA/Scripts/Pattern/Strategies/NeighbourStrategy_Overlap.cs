using System;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourStrategy_Overlap : IFindNeighbourStrategy
{
    public Dictionary<int, PatternNeighbours> FindNeighbours( PatternDataResults patternDataResults ) {
        Dictionary<int, PatternNeighbours> result = new Dictionary<int, PatternNeighbours>();

        foreach( KeyValuePair<int, PatternData> patternDataPair in patternDataResults.PatternDataIndexDictionary )
            foreach( KeyValuePair<int, PatternData> patternDataPair2 in patternDataResults.PatternDataIndexDictionary )
                FindNeighboursInAllDirections( result, patternDataPair, patternDataPair2 );
        
        // DEBUG START
        // foreach( KeyValuePair<int, PatternNeighbours> res in result )
        //     UnityEngine.Debug.Log(res.Key + "->\n" + res.Value.ToString());
        // DEBUG END

        return result;
    }

    private void FindNeighboursInAllDirections( Dictionary<int, PatternNeighbours> result, KeyValuePair<int, PatternData> patternDataPair, KeyValuePair<int, PatternData> patternDataPair2 ) {
        foreach( Direction dir in Enum.GetValues( typeof( Direction ) ) )
            if( patternDataPair.Value.CompareGrid( dir, patternDataPair2.Value ) ) {
                if( result.ContainsKey( patternDataPair.Key ) == false )
                    result.Add( patternDataPair.Key, new PatternNeighbours() );
                
                result[ patternDataPair.Key ].AddPattern( dir, patternDataPair2.Key );
            }
    }
}
