public struct Coords
{
    public Coords(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public override string ToString() => $"({X}, {Y})";

    public bool IsSame(Coords p)
    {
        return (X == p.X && Y == p.Y);
    }
}