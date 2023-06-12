using UnityEngine.Tilemaps;

public class TileBaseValue : IValue<TileBase>
{
    private TileBase tileBase;

    public TileBaseValue( TileBase tileBase ) {
        this.tileBase = tileBase;
    }

    public TileBase Value => tileBase;

    public bool Equals( IValue<TileBase> x, IValue<TileBase> y ) => x == y;

    public bool Equals( IValue<TileBase> other ) => other.Value == this.Value;

    public int GetHashCode( IValue<TileBase> obj ) => obj.GetHashCode();
    public override int GetHashCode() => tileBase.GetHashCode();
}
