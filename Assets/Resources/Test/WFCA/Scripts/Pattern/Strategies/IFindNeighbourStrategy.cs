using System.Collections.Generic;

public interface IFindNeighbourStrategy
{
    Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternDataResults);
}
