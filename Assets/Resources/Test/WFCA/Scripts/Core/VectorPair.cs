using UnityEngine;

public class VectorPair
{
    public Vector2Int CellPosition { get; set; }
    public Vector2Int NextCellPosition { get; set; }
    public Vector2Int PreviousCellPosition { get; set; }
    public Direction DirectionFromBase { get; set; }

    public VectorPair( Vector2Int cellPosition, Vector2Int nextCellPosition, Vector2Int previousCellPosition, Direction directionFromBase ) {
        CellPosition = cellPosition;
        NextCellPosition = nextCellPosition;
        PreviousCellPosition = previousCellPosition;
        DirectionFromBase = directionFromBase;
    }

    public bool AreNextAndPreviousCellsSame() => NextCellPosition == PreviousCellPosition;
}
