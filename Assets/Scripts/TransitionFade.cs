using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionFade : MonoBehaviour
{
    bool startFadeOut = false;
    bool startFadeIn = false;
    void OnEnable()
    {
        startFadeIn = true;
        startFadeOut = false;
    }

    void Update()
    {
        if (startFadeIn)
        {
            FadeIn();
        }
        else if (startFadeOut)
        {
            FadeOut();
        }
    }

    void FadeIn()
    {
        if (gameObject.GetComponent<Image>().color.a < 1)
        {
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, gameObject.GetComponent<Image>().color.a + 1f);
        }
        else
        {
            startFadeIn = false;
            startFadeOut = false;
            StartCoroutine(WaitForHold());
        }
    }

    IEnumerator WaitForHold()
    {
        yield return new WaitForSeconds(2);
        startFadeOut = true;
    }

    void FadeOut()
    {
        if (gameObject.GetComponent<Image>().color.a > 0)
        {
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, gameObject.GetComponent<Image>().color.a - 0.01f);
        }
        else
        {
            startFadeIn = false;
            startFadeOut = false;
            gameObject.SetActive(false);
        }
    }
}
