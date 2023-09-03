using UnityEngine;
using UnityEngine.InputSystem;

public class TurnTimeController : MonoBehaviour
{
    public static TurnTimeController instance { get; private set; }

    public delegate void TurnHandler(PlayerAction action);
    public event TurnHandler NextTurn;

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
        NextTurn.Invoke(PlayerAction.MOVE_UP);
    }
    private void OnMoveDown(InputAction.CallbackContext input)
    {
        if (stopped) return;
        NextTurn.Invoke(PlayerAction.MOVE_DOWN);
    }
    private void OnMoveLeft(InputAction.CallbackContext input)
    {
        if (stopped) return;
        NextTurn.Invoke(PlayerAction.MOVE_LEFT);
    }
    private void OnMoveRight(InputAction.CallbackContext input)
    {
        if (stopped) return;
        NextTurn.Invoke(PlayerAction.MOVE_RIGHT);
    }
    private void OnAttack(InputAction.CallbackContext input)
    {
        if (stopped) return;
        NextTurn.Invoke(PlayerAction.ATTACK);
    }

    public void Stop()
    {
        stopped = true;
    }

    public void Resume()
    {
        stopped = false;
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
