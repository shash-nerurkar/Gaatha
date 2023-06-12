using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PatternNeighbours
{
    private Dictionary<Direction, HashSet<int>> directionNeighboursDictionary = new Dictionary<Direction, HashSet<int>>();

    public void AddPattern( Direction dir, int patternIndex ) {
        if( directionNeighboursDictionary.ContainsKey( dir ) )
            directionNeighboursDictionary[ dir ].Add( patternIndex );
        else
            directionNeighboursDictionary.Add( dir, new HashSet<int>() { patternIndex } );
    }

    internal HashSet<int> GetNeighboursInDirection( Direction dir ) {
        if( directionNeighboursDictionary.ContainsKey( dir ) )
            return directionNeighboursDictionary[ dir ];
        else
            return new HashSet<int>();
    }

    public void AddNeighbour( PatternNeighbours neighbours ) {
        foreach( KeyValuePair<Direction, HashSet<int>> item in neighbours.directionNeighboursDictionary ) {
            if( directionNeighboursDictionary.ContainsKey( item.Key ) == false )
                directionNeighboursDictionary.Add( item.Key, new HashSet<int>() );
            
            directionNeighboursDictionary[ item.Key ].UnionWith( item.Value );
        }
    }

    override public string ToString() {
        StringBuilder builder = new StringBuilder();
        
        foreach( KeyValuePair<Direction, HashSet<int>> pair in directionNeighboursDictionary ) {
            builder.Append( pair.Key.ToString() + ": " + string.Join( " ", pair.Value.ToArray() ) + "\n" );
        }

        return builder.ToString();
    }
}
