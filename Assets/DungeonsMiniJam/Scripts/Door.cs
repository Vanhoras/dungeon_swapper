using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Tile
{
    [SerializeField] 
    private string scene;

    [SerializeField]
    private bool hasLock = false;

    [SerializeField]
    private GameObject lockGameObject;

    private Coords coords;

    private void Start()
    {
        base.Start();

        TurnController.instance.PlayerEndTurn += OnNextTurn;
        coords = GridController.instance.DetermineCoords(transform);

        lockGameObject.SetActive(hasLock);
        if (hasLock) walkable = false;
    }

    private void OnDestroy()
    {
        TurnController.instance.PlayerEndTurn -= OnNextTurn;
    }


    private void OnNextTurn(Player player)
    {
        if (coords.IsSame(player.GetCoords()))
        {
            if (!hasLock)
            {
                SceneManager.LoadScene(scene);
            }
        }
    }

    public void Unlock()
    {
        lockGameObject.SetActive(false);
        hasLock = false;
        walkable = true;
    }
}
