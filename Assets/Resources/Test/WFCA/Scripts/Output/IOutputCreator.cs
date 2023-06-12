public interface IOutputCreator<T>
{
    public T OutputImage { get; }

    public void CreateOutput( PatternManager patternManager, int[ ][ ] outputValues, int width, int height );
}
