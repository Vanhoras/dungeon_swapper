using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    [SerializeField]
    protected bool walkable;

    [SerializeField]
    protected bool cover;

    protected Coords coords;

    protected void Start()
    {
        GridController.instance.AddObstacle(this);

        coords = GridController.instance.DetermineCoords(transform);
    }

    public void Swap(Coords newCoords)
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
