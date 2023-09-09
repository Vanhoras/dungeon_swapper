using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private Door doorUnlocked;

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


    private void OnNextTurn(Player player)
    {
        if (coords.IsSame(player.GetCoords()))
        {
            doorUnlocked.Unlock();
            Destroy(this.gameObject);
        }
    }
}
