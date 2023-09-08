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
    private Obstacle[,] obstacles;

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
        obstacles = new Obstacle[tilesCountX, tilesCountY];
    }

    public int GetTilesCountX()
    {
        return tilesCountX;
    }

    public int GetTilesCountY()
    {
        return tilesCountY;
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

    public void RemoveObstacle(Obstacle obstacle)
    {
        Coords obstacleCoords = DetermineCoords(obstacle.transform);

        obstacles[obstacleCoords.X, obstacleCoords.Y] = null;
    }

    public void AddObstacle(Obstacle obstacle)
    {
        Coords obstacleCoords = DetermineCoords(obstacle.transform);

        obstacles[obstacleCoords.X, obstacleCoords.Y] = obstacle;
    }

    public Obstacle GetObstacleAtCoord(Coords coords)
    {
        return obstacles[coords.X, coords.Y];
    }

    public GameObject FindObstacleInDirection(Coords coords, Direction direction)
    {
        Tile tempTile;
        int y;
        int x;

        switch (direction)
        {
            case Direction.UP:
                y = coords.Y - 1;
                while(y > -1)
                {
                    Coords currentCoords = new Coords(coords.X, y);
                    Obstacle obstacle = GetObstacleAtCoord(currentCoords);

                    if (obstacle != null) return obstacle.gameObject;

                    tempTile = GetTileAtCoord(currentCoords);

                    if (tempTile.IsCover()) return tempTile.gameObject;

                    y--;
                }
                break;
            case Direction.DOWN:
                y = coords.Y + 1;
                while (y < tilesCountY)
                {
                    Coords currentCoords = new Coords(coords.X, y);
                    Obstacle obstacle = GetObstacleAtCoord(currentCoords);

                    if (obstacle != null) return obstacle.gameObject;

                    tempTile = GetTileAtCoord(currentCoords);

                    if (tempTile.IsCover()) return tempTile.gameObject;

                    y++;
                }
                break;
            case Direction.LEFT:
                x = coords.X - 1;
                while (x > -1)
                {
                    Coords currentCoords = new Coords(x, coords.Y);
                    Obstacle obstacle = GetObstacleAtCoord(currentCoords);

                    if (obstacle != null) return obstacle.gameObject;

                    tempTile = GetTileAtCoord(currentCoords);

                    if (tempTile.IsCover()) return tempTile.gameObject;

                    x--;
                }
                break;
            case Direction.RIGHT:
                x = coords.X + 1;
                while (x < tilesCountX)
                {
                    Coords currentCoords = new Coords(x, coords.Y);
                    Obstacle obstacle = GetObstacleAtCoord(currentCoords);

                    if (obstacle != null) return obstacle.gameObject;

                    tempTile = GetTileAtCoord(currentCoords);

                    if (tempTile.IsCover()) return tempTile.gameObject;

                    x++;
                }
                break;
        }
        return null;
    }

    public bool IsCoordsCover(Coords coords)
    {
        Obstacle obstacle = GetObstacleAtCoord(coords);

        if (obstacle != null && obstacle.IsCover()) return true;

        Tile tile = GetTileAtCoord(coords);

        if (tile != null && tile.IsCover()) return true;

        return false;
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

    public bool IsSame(Coords p)
    {
        return (X == p.X && Y == p.Y);
    }
}
