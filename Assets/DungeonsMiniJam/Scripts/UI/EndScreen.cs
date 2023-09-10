using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject blackScreen;

    [SerializeField]
    private GameObject credits;

    [SerializeField]
    private float fadeSpeed;

    public void StartFading(string ending)
    {
        TurnController.instance.Stop();

        blackScreen.SetActive(true);
        StartCoroutine(FadeBlackScreen());
    }

    public IEnumerator FadeBlackScreen()
    {
        Color objectColor = blackScreen.GetComponent<Image>().color;
        float fadeAmount;

        while (blackScreen.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackScreen.GetComponent<Image>().color = objectColor;
            yield return null;
        }

        credits.SetActive(true);
    }
}
