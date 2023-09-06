using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] 
    private string scene;

    private Coords coords;

    private void Start()
    {
        TurnController.instance.PlayerEndTurn += OnNextTurn;
        coords = GridController.instance.DetermineCoords(transform);
    }

    private void OnDestroy()
    {
        TurnController.instance.PlayerEndTurn -= OnNextTurn;
    }


    private void OnNextTurn(Coords playerCoords)
    {
        if (coords.IsSame(playerCoords))
        {
            SceneManager.LoadScene(scene);
        }
    }
}