using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StepController : MonoBehaviour
{
    public static StepController instance { get; private set; }

    private DungeonsControls inputActions;


    private Dictionary<int, ISaveable> saveables = new Dictionary<int, ISaveable>();

    private List<Step> steps = new List<Step>();

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
        inputActions.Global.Enable();
        inputActions.Global.StepBack.performed += OnStepBack;

        TurnController.instance.StartTurn += SaveStep;
    }

    private void OnDestroy()
    {
        inputActions.Global.StepBack.performed -= OnStepBack;

        TurnController.instance.StartTurn -= SaveStep;
    }

    public void AddSaveable(ISaveable saveable, int id)
    {
        saveables.Add(id, saveable);
    }

    public void SaveStep()
    {
        Dictionary<int, State> states = new Dictionary<int, State>();
        foreach (KeyValuePair<int, ISaveable> entry in saveables)
        {
            states.Add(entry.Key, entry.Value.GetState());
        }

        Step step = new Step(states);

        steps.Add(step);
    }

    private void OnStepBack(InputAction.CallbackContext input)
    {
        LoadLastStep();
    }

    public void LoadLastStep()
    {
        if (TurnController.instance.IsStopped() && !TurnController.instance.GameOver) return;
        if (steps.Count == 0) return;

        Step lastStep = steps[steps.Count - 1];

        // Case that Saveable is not found is not handled. Saveable Objects may not be deleted.
        foreach (KeyValuePair<int, State> entry in lastStep.states)
        {
            ISaveable saveable = saveables[entry.Key];

            saveable.SetState(entry.Value);
        }

        steps.RemoveAt(steps.Count - 1);
    }

}

public struct Step
{
    public Dictionary<int, State> states;

    public Step(Dictionary<int, State> states)
    {
        this.states = states;
    }

}
