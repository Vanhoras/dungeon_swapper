using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance { get; private set; }

    [SerializeField]
    private TMP_Text ResetTutorial;

    [SerializeField]
    private float fadeSpeed;

    private IEnumerator coroutine;

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

    public void FlickerResetTutorial()
    {
        coroutine = FlickerResetScreen();
        StartCoroutine(coroutine);
    }

    public void StopFlickerResetTutorial()
    {
        StopCoroutine(coroutine);
        ResetTutorial.color = new Color(ResetTutorial.color.r, ResetTutorial.color.g, ResetTutorial.color.b, 1);
    }

    public IEnumerator FlickerResetScreen()
    {
        Color tutorialColor = ResetTutorial.color;

        float fadeAmount;
        bool fadingOut = true;

        while (true)
        {
            if (fadingOut && tutorialColor.a <= 0.05)
            {
                fadingOut = false;
            } else if (!fadingOut && tutorialColor.a >= 1)
            {
                fadingOut = true;
            }

            fadeAmount = fadingOut ? 
                tutorialColor.a - (fadeSpeed * Time.deltaTime) : 
                tutorialColor.a + (fadeSpeed * Time.deltaTime);

            tutorialColor = new Color(tutorialColor.r, tutorialColor.g, tutorialColor.b, fadeAmount);
            ResetTutorial.color = tutorialColor;
            yield return null;
        }
    }
}
