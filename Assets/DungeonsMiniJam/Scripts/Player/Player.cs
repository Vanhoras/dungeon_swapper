using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, ISaveable
{

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private Vector3 projectileSpawnUp;

    [SerializeField]
    private Vector3 projectileSpawnDown;

    [SerializeField]
    private Vector3 projectileSpawnRight;

    [SerializeField]
    private Vector3 projectileSpawnLeft;


    private PlayerState state;

    private Animator animator;


    private void Start()
    {
        StepController.instance.AddSaveable(this, GetInstanceID());

        animator = GetComponent<Animator>();

        TurnController.instance.PlayerTurn += OnNextTurn;


        state.coords = GridController.instance.DetermineCoords(this.transform);
        state.direction = Direction.RIGHT;
        state.dead = false;
    }

    private void OnDestroy()
    {
        TurnController.instance.PlayerTurn -= OnNextTurn;
    }

    public State GetState()
    {
        return state;
    }

    public void SetState(State state)
    {
        if (state == null || state is not PlayerState) return;

        PlayerState newState = (PlayerState) state;

        if (this.state.dead && !newState.dead)
        {
            Revive();
        }

        this.state = newState;

        transform.position = GridController.instance.GetPositionOfCoord(this.state.coords);
        SetDirection(this.state.direction);
    }

    public Coords GetCoords()
    {
        return state.coords;
    }

    public void Die()
    {
        TurnController.instance.Stop();
        TurnController.instance.GameOver = true;

        state.dead = true;
        animator.SetTrigger("Death");
        Tutorial.instance.FlickerResetTutorial();
    }

    public void Revive()
    {
        state.dead = false;
        animator.SetTrigger("Revive");
        Tutorial.instance.StopFlickerResetTutorial();

        TurnController.instance.Resume();
        TurnController.instance.GameOver = false;
    }

    private void OnNextTurn(PlayerAction action)
    {

        if (action == PlayerAction.ATTACK)
        {
            StartSwap();
        } else if (action == PlayerAction.MOVE_UP) 
        {
            MoveToCoords(new Coords(state.coords.X, state.coords.Y - 1), Direction.UP);
        } else if (action == PlayerAction.MOVE_DOWN)
        {
            MoveToCoords(new Coords(state.coords.X, state.coords.Y + 1), Direction.DOWN);
        } else if (action == PlayerAction.MOVE_LEFT)
        {
            MoveToCoords(new Coords(state.coords.X - 1, state.coords.Y), Direction.LEFT);
        }  else if (action == PlayerAction.MOVE_RIGHT)
        {
            MoveToCoords(new Coords(state.coords.X + 1, state.coords.Y), Direction.RIGHT);
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
            TurnController.instance.EndPlayerTurn(this);
            return;
        }

        state.coords = newCoords;
        transform.position = GridController.instance.GetPositionOfCoord(newCoords);
        TurnController.instance.EndPlayerTurn(this);
    }

    private void SetDirection(Direction newDirection)
    {
        state.direction = newDirection;

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

    private void StartSwap()
    {
        GameObject objectHit = GridController.instance.FindObstacleInDirection(state.coords, state.direction);

        if (objectHit == null) return;

        Obstacle obstacle = objectHit.GetComponent<Obstacle>();

        Vector3 projectileGoal = objectHit.transform.position;

        Vector3 startPosition = projectileSpawnRight;

        switch (state.direction)
        {
            case Direction.UP:
                startPosition = projectileSpawnUp;
                break;
            case Direction.DOWN:
                startPosition = projectileSpawnDown;
                break;
            case Direction.LEFT:
                startPosition = projectileSpawnLeft;
                break;
            case Direction.RIGHT:
                startPosition = projectileSpawnRight;
                break;
        }

        StartCoroutine(MoveSwapProjectile(projectileGoal, transform.TransformPoint(startPosition), obstacle));

    }

    private void FinishSwap(Obstacle obstacle)
    {

        if (obstacle == null)
        {
            TurnController.instance.EndPlayerTurn(this);
            return;
        }

        Coords obstacleCoords = obstacle.GetCoords();

        obstacle.MoveTo(state.coords);

        MoveToCoords(obstacleCoords, state.direction);
    }

    private IEnumerator MoveSwapProjectile(Vector3 goal, Vector3 startLocation, Obstacle obstacle)
    {
        TurnController.instance.Stop();
        GameObject projectile = Instantiate(projectilePrefab, startLocation, Quaternion.identity);

        while (Vector3.Distance(projectile.transform.position, goal) > 0.01f)
        {

            var step = projectileSpeed * Time.deltaTime;
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, goal, step);

            yield return null;
        }

        Destroy(projectile);
        TurnController.instance.Resume();
        FinishSwap(obstacle);
        yield return null;
    }
}

public struct PlayerState : State
{
    public Coords coords;
    public Direction direction;
    public bool dead;
}


