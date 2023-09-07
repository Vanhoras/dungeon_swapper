using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance { get; private set; }

    [SerializeField]
    private GameObject ResetTutorial;

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

    public void ShowTutorial()
    {
        ResetTutorial.SetActive(true);
    }
}
