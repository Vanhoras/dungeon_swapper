using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Door : Tile
{
    [SerializeField] 
    private string scene;

    [SerializeField]
    private bool hasLock = false;

    [SerializeField]
    private UnityEvent unityEvent;

    [SerializeField]
    private GameObject lockGameObject;

    private Coords coords;

    private void Start()
    {
        base.Start();

        TurnController.instance.EnemyTurn += OnNextTurn;
        coords = GridController.instance.DetermineCoords(transform);

        lockGameObject.SetActive(hasLock);
        if (hasLock) walkable = false;
    }

    private void OnDestroy()
    {
        TurnController.instance.EnemyTurn -= OnNextTurn;
    }


    private void OnNextTurn(Player player)
    {
        if (coords.IsSame(player.GetCoords()))
        {
            if (!hasLock)
            {
                unityEvent?.Invoke();

                if (scene != "") SceneManager.LoadScene(scene);
            }
        }
    }

    public void Unlock()
    {
        lockGameObject.SetActive(false);
        hasLock = false;
        walkable = true;
    }

    public void Lock()
    {
        lockGameObject.SetActive(true);
        hasLock = true;
        walkable = false;
    }
}
