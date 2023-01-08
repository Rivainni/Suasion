using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeController : MonoBehaviour
{
    [SerializeField] float timeMultiplier;
    [SerializeField] float startHour;
    [SerializeField] TextMeshProUGUI timeIndicator;
    [SerializeField] GameManager gameManager;
    DateTime currentTime;
    bool pause;
    // Start is called before the first frame update
    void Start()
    {
        ResetTime();
        pause = false;
    }

    void Update()
    {
        if (!pause)
        {
            currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
            timeIndicator.text = currentTime.ToString("HH:mm");
        }

        if (timeIndicator.text == "21:00")
        {
            gameManager.AddLevel();
            ResetTime();
        }
    }

    void ResetTime()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        timeIndicator.text = currentTime.ToString("HH:mm");
    }

    public void SetPause(bool pause)
    {
        this.pause = pause;
    }
}