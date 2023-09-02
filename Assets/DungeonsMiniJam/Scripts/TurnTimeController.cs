using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnTimeController : MonoBehaviour
{
    public static TurnTimeController instance { get; private set; }

    public delegate void TurnHandler();
    public event TurnHandler OnNextTurn;

    [SerializeField]
    private float turnTime;

    private float timeTillNextTurn;

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timeTillNextTurn = turnTime;
    }

    private void Start()
    {
        timeTillNextTurn = turnTime;
    }

    private void Update()
    {
        timeTillNextTurn -= Time.deltaTime;

        if (timeTillNextTurn <= 0) { 
            timeTillNextTurn = turnTime;

            OnNextTurn.Invoke();
        }
    }
}
