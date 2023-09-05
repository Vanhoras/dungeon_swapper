using UnityEngine;

public class Player : MonoBehaviour
{

    private Coords coords;

    private Direction direction;

    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();

        TurnTimeController.instance.NextTurn += OnNextTurn;

        coords = GridController.instance.DetermineCoords(this.transform);

        direction = Direction.RIGHT;
    }

    private void OnDestroy()
    {
        TurnTimeController.instance.NextTurn -= OnNextTurn;
    }

    private void OnNextTurn(PlayerAction action)
    {

        if (action == PlayerAction.ATTACK)
        {
            Swap();
        } else if (action == PlayerAction.MOVE_UP) 
        {
            MoveToCoords(new Coords(coords.X, coords.Y - 1), Direction.UP);
        } else if (action == PlayerAction.MOVE_DOWN)
        {
            MoveToCoords(new Coords(coords.X, coords.Y + 1), Direction.DOWN);
        } else if (action == PlayerAction.MOVE_LEFT)
        {
            MoveToCoords(new Coords(coords.X - 1, coords.Y), Direction.LEFT);
        }  else if (action == PlayerAction.MOVE_RIGHT)
        {
            MoveToCoords(new Coords(coords.X + 1, coords.Y), Direction.RIGHT);
        }
    }

    private void MoveToCoords(Coords newCoords, Direction newDirection)
    {
        SetDirection(newDirection);

        bool tileWalkable = GridController.instance.GetTileAtCoord(newCoords).IsWalkable();
        Obstacle obstacle = GridController.instance.GetObstacleAtCoord(newCoords);
        bool obstacleWalkable = obstacle != null ? obstacle.IsWalkable() : true;

        if ( !(tileWalkable && obstacleWalkable))
        {
            return;
        }

        coords = newCoords;
        transform.position = GridController.instance.GetPositionOfCoord(newCoords);
    }

    private void SetDirection(Direction newDirection)
    {
        direction = newDirection;

        switch (newDirection)
        {
            case Direction.UP:
                animator.SetBool("Up", true);
                animator.SetBool("Down", false);
                transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                break;
            case Direction.DOWN:
                animator.SetBool("Up", false);
                animator.SetBool("Down", true);
                transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                break;
            case Direction.LEFT:
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                transform.transform.eulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
                break;
            case Direction.RIGHT:
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                break;
        }
        
    }

    private void Swap()
    {
        Debug.Log("Swap: " + direction);
        Obstacle obstacle = GridController.instance.FindObstacleInDirection(coords, direction);

        if (obstacle == null) return;

        Debug.Log("Found Obstacle");

        Coords obstacleCoords = obstacle.GetCoords();

        obstacle.Swap(coords);

        MoveToCoords(obstacleCoords, direction);
    }
}


