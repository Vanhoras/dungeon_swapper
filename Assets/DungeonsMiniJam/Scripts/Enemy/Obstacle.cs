using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    [SerializeField]
    protected bool walkable;

    [SerializeField]
    protected bool cover;

    private bool originalWalkable;
    private bool originalCover;

    public Coords coords;

    protected void Start()
    {
        GridController.instance.AddObstacle(this);

        coords = GridController.instance.DetermineCoords(transform);

        originalWalkable = walkable;
        originalCover = cover;
    }

    public void MoveTo(Coords newCoords)
    {
        GridController.instance.RemoveObstacle(this);

        transform.position = GridController.instance.GetPositionOfCoord(newCoords);
        coords = newCoords;

        GridController.instance.AddObstacle(this);
    }

    protected void RemoveAsObstacle()
    {
        cover = false;
        walkable = true;
        GridController.instance.RemoveObstacle(this);
    }

    protected void ReAddAsObstacle()
    {
        cover = originalCover;
        walkable = originalWalkable;
        GridController.instance.AddObstacle(this);
    }

    public bool IsWalkable()
    {
        return walkable;
    }

    public bool IsCover()
    {
        return cover;
    }

    public Coords GetCoords()
    {
        return coords;
    }
}


public class ObstacleState : State
{
    public Coords coords { get; set; }

    public ObstacleState(Coords coords) {
        this.coords = coords;
    }
}