using UnityEngine;
using UnityEngine.InputSystem;

public class TurnController : MonoBehaviour
{
    public static TurnController instance { get; private set; }

    public delegate void PlayerActionDelegate(PlayerAction action);
    public event PlayerActionDelegate PlayerStartTurn;

    public delegate void PlayerPositionDelegate(Coords playerCoords);
    public event PlayerPositionDelegate PlayerEndTurn;

    private DungeonsControls inputActions;

    private bool stopped;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        inputActions = new DungeonsControls();
        inputActions.Player.Enable();
        inputActions.Player.MoveUp.performed += OnMoveUp;
        inputActions.Player.MoveDown.performed += OnMoveDown;
        inputActions.Player.MoveLeft.performed += OnMoveLeft;
        inputActions.Player.MoveRight.performed += OnMoveRight;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDestroy()
    {
        inputActions.Player.MoveUp.performed -= OnMoveUp;
        inputActions.Player.MoveDown.performed -= OnMoveDown;
        inputActions.Player.MoveLeft.performed -= OnMoveLeft;
        inputActions.Player.MoveRight.performed -= OnMoveRight;
        inputActions.Player.Attack.performed -= OnAttack;
    }

    private void OnMoveUp(InputAction.CallbackContext input)
    {
        if (stopped) return;
        PlayerStartTurn.Invoke(PlayerAction.MOVE_UP);
    }
    private void OnMoveDown(InputAction.CallbackContext input)
    {
        if (stopped) return;
        PlayerStartTurn.Invoke(PlayerAction.MOVE_DOWN);
    }
    private void OnMoveLeft(InputAction.CallbackContext input)
    {
        if (stopped) return;
        PlayerStartTurn.Invoke(PlayerAction.MOVE_LEFT);
    }
    private void OnMoveRight(InputAction.CallbackContext input)
    {
        if (stopped) return;
        PlayerStartTurn.Invoke(PlayerAction.MOVE_RIGHT);
    }
    private void OnAttack(InputAction.CallbackContext input)
    {
        if (stopped) return;
        PlayerStartTurn.Invoke(PlayerAction.ATTACK);
    }

    public void Stop()
    {
        stopped = true;
    }

    public void Resume()
    {
        stopped = false;
    }

    public void EndPlayerTurn(Coords playerCoords)
    {
        PlayerEndTurn.Invoke(playerCoords);
    }
}

public enum PlayerAction
{
    MOVE_UP,
    MOVE_DOWN, 
    MOVE_LEFT, 
    MOVE_RIGHT,
    ATTACK,
}
