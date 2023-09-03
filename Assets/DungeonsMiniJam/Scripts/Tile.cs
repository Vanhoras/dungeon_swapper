using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private bool walkable;

    [SerializeField]
    private bool cover;

    private void Start()
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
