using UnityEngine;

public class PatternData
{
    private Pattern pattern;
    private int frequency;
    private float frequencyRelative;
    private float frequencyRelativeLog2;

    public Pattern Pattern { get => pattern; }
    public float FrequencyRelative { get => frequencyRelative; }
    public float FrequencyRelativeLog2 { get => frequencyRelativeLog2; }

    public PatternData( Pattern pattern ) {
        this.pattern = pattern;

        this.frequency = 1;
    }

    public void IncreaseFrequency() => ++frequency;

    public void CalculateRelativeFrequency( int total ) {
        frequencyRelative = ( float )frequency / total;
        frequencyRelativeLog2 = Mathf.Log( frequencyRelative, 2 );
    }

    public bool CompareGrid( Direction dir, PatternData data ) {
        return pattern.ComparePattern( dir, data.pattern );
    }
}
