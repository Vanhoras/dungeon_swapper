using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetController : MonoBehaviour
{
    public static ResetController instance { get; private set; }

    private DungeonsControls inputActions;

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
        inputActions.Global.Enable();
        inputActions.Global.Reset.performed += OnReset;
    }

    private void OnReset(InputAction.CallbackContext input)
    {
       Reset();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
