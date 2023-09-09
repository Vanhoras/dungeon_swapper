using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    protected bool walkable;

    [SerializeField]
    protected bool cover;

    protected void Start()
    {
        GridController.instance.AddTile(this);
    }

    public bool IsWalkable()
    {
        return walkable;
    }

    public bool IsCover()
    {
        return cover;
    }
}
