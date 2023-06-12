using System;
using System.Collections.Generic;
using UnityEngine;

public class LowEntropyCell : IComparable<LowEntropyCell>, IEqualityComparer<LowEntropyCell>
{
    public Vector2Int Position { get; set; }
    public float Entropy { get; set; }
    private float smallEntropyNoise;

    public LowEntropyCell( Vector2Int position, float entropy ) {
        smallEntropyNoise = UnityEngine.Random.Range( 0.001f, 0.005f );

        Position = position;
        Entropy = entropy + smallEntropyNoise;
    }

    public int CompareTo( LowEntropyCell other ) {
        if( Entropy > other.Entropy )
            return 1;
        else if( Entropy < other.Entropy )
            return -1;
        else   
            return 0;
    }

    bool IEqualityComparer<LowEntropyCell>.Equals( LowEntropyCell cell1, LowEntropyCell cell2 ) => cell1 == cell2; 

    int IEqualityComparer<LowEntropyCell>.GetHashCode( LowEntropyCell obj ) => obj.GetHashCode();

    public override int GetHashCode() => Position.GetHashCode();
}
