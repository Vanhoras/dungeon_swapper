using UnityEngine;

public class Key : MonoBehaviour, ISaveable
{
    [SerializeField]
    private Door doorUnlocked;

    [SerializeField]
    private SpriteRenderer keyImage;

    private KeyState state;

    private void Start()
    {
        TurnController.instance.EnemyTurn += OnNextTurn;
        StepController.instance.AddSaveable(this, GetInstanceID());

        state = new KeyState(GridController.instance.DetermineCoords(transform), false);
    }

    private void OnDestroy()
    {
        TurnController.instance.EnemyTurn -= OnNextTurn;
    }

    private void OnNextTurn(Player player)
    {
        if (state.coords.IsSame(player.GetCoords()))
        {
            Collect();
        }
    }

    private void Collect()
    {
        state.collected = true;
        doorUnlocked.Unlock();
        keyImage.gameObject.SetActive(false);
    }

    private void UnCollect()
    {
        state.collected = false;
        doorUnlocked.Lock();
        keyImage.gameObject.SetActive(true);
    }

    public State GetState()
    {
        return state;
    }

    public void SetState(State state)
    {
        if (state == null || state is not KeyState) return;

        KeyState newState = (KeyState)state;

        this.state = newState;

        transform.position = GridController.instance.GetPositionOfCoord(this.state.coords);

        if (this.state.collected)
        {
            Collect();
        } else
        {
            UnCollect();
        }
    }
}

public struct KeyState : State
{
    public Coords coords;
    public bool collected;

    public KeyState(Coords coords, bool collected)
    {
        this.coords = coords;
        this.collected = collected;
    }
}
