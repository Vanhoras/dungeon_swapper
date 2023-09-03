using UnityEngine;

public class GridController : MonoBehaviour
{
    public static GridController instance { get; private set; }

    [SerializeField]
    private float tileWidth;

    [SerializeField]
    private float tileHeight;

    [SerializeField]
    private int tilesCountX;

    [SerializeField]
    private int tilesCountY;

    private Tile[,] tiles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        tiles = new Tile[tilesCountX, tilesCountY];
    }

    public void AddTile(Tile tile)
    {
        Coords tileCoords = DetermineCoords(tile.transform);

        tiles[tileCoords.X, tileCoords.Y] = tile;
    }

    public Tile GetTileAtCoord(Coords coords)
    {
        return tiles[coords.X, coords.Y];
    }

    // Only works when y coordinates are negative.
    // Due to time constraint I'll leave this solution for now.
    public Vector2 GetPositionOfCoord(Coords coords)
    {
        float x = coords.X * tileWidth;
        float y = -1 * coords.Y * tileHeight;

        return new Vector2(x, y);
    }

    public Coords DetermineCoords(Transform targetTransform)
    {
        int coordX = (int)(targetTransform.position.x / tileWidth);
        int coordY = Mathf.Abs((int)(targetTransform.position.y / tileHeight));

        return new Coords(coordX, coordY);
    }

    public Coords DetermineCoords(float x, float y)
    {
        int coordX = (int)(x / tileWidth);
        int coordY = Mathf.Abs((int)(y / tileHeight));

        return new Coords(coordX, coordY);
    }
}

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
}
