using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFade : MonoBehaviour
{
    bool startFadeOut = false;
    bool startFadeIn = false;
    void OnEnable()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
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
        if (gameObject.GetComponent<CanvasGroup>().alpha < 1)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = gameObject.GetComponent<CanvasGroup>().alpha + 0.005f;
        }
        else
        {
            startFadeIn = false;
            startFadeOut = false;
        }
    }

    void FadeOut()
    {
        if (gameObject.GetComponent<CanvasGroup>().alpha > 0)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = gameObject.GetComponent<CanvasGroup>().alpha - 0.005f;
        }
        else
        {
            startFadeIn = false;
            startFadeOut = false;
            gameObject.SetActive(false);
        }
    }

    public void StartFadeIn()
    {
        startFadeIn = true;
        startFadeOut = false;
    }

    public void StartFadeOut()
    {
        startFadeIn = false;
        startFadeOut = true;
    }
}
