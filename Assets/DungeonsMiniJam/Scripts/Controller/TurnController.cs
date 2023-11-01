using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnController : MonoBehaviour
{
    public static TurnController instance { get; private set; }

    // save game
    public delegate void StarTurnDelegate();
    public event StarTurnDelegate StartTurn;

    // player acts
    public delegate void PlayerActionDelegate(PlayerAction action);
    public event PlayerActionDelegate PlayerTurn;

    // enemies act
    public delegate void PlayerDelegate(Player player);
    public event PlayerDelegate EnemyTurn;

    private DungeonsControls inputActions;

    private bool stopped = false;

    [NonSerialized]
    public bool GameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        StartTurn.Invoke();
        PlayerTurn.Invoke(PlayerAction.MOVE_UP);
    }
    private void OnMoveDown(InputAction.CallbackContext input)
    {
        if (stopped) return;
        StartTurn.Invoke();
        PlayerTurn.Invoke(PlayerAction.MOVE_DOWN);
    }
    private void OnMoveLeft(InputAction.CallbackContext input)
    {
        if (stopped) return;
        StartTurn.Invoke();
        PlayerTurn.Invoke(PlayerAction.MOVE_LEFT);
    }
    private void OnMoveRight(InputAction.CallbackContext input)
    {
        if (stopped) return;
        StartTurn.Invoke();
        PlayerTurn.Invoke(PlayerAction.MOVE_RIGHT);
    }
    private void OnAttack(InputAction.CallbackContext input)
    {
        if (stopped) return;
        StartTurn.Invoke();
        PlayerTurn.Invoke(PlayerAction.ATTACK);
    }

    public void Stop()
    {
        stopped = true;
    }

    public void Resume()
    {
        stopped = false;
    }

    public bool IsStopped()
    {
        return stopped;
    }

    public void EndPlayerTurn(Player player)
    {
        EnemyTurn.Invoke(player);
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
