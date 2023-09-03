using UnityEngine;

public class Player : MonoBehaviour
{

    private Coords coords;


    private void Start()
    {
        TurnTimeController.instance.NextTurn += OnNextTurn;

        coords = GridController.instance.DetermineCoords(this.transform);
    }

    private void OnDestroy()
    {
        TurnTimeController.instance.NextTurn -= OnNextTurn;
    }

    private void OnNextTurn(PlayerAction action)
    {
        Debug.Log("Next Turn");

        if (action == PlayerAction.ATTACK)
        {
            // TODO
        } else if (action == PlayerAction.MOVE_UP) 
        {
            MoveToCoords(new Coords(coords.X, coords.Y - 1));
        } else if (action == PlayerAction.MOVE_DOWN)
        {
            MoveToCoords(new Coords(coords.X, coords.Y + 1));
        } else if (action == PlayerAction.MOVE_LEFT)
        {
            MoveToCoords(new Coords(coords.X + 1, coords.Y));
        }  else if (action == PlayerAction.MOVE_RIGHT)
        {
            MoveToCoords(new Coords(coords.X - 1, coords.Y));
        }
    }

    private void MoveToCoords(Coords newCoords)
    {
        if ( ! GridController.instance.GetTileAtCoord(newCoords).IsWalkable())
        {
            return;
        }

        coords = newCoords;
        transform.position = GridController.instance.GetPositionOfCoord(newCoords);
    }


}
