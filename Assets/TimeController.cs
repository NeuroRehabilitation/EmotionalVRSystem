using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private float elapsedTime;
    public bool isRunning;
    public float duration = 0.2f; //duration of experiment in minutes.

    public Countdown Countdown;


    private void Update()
    {
        Debug.Log(isRunning);
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
        }

        if (isRunning && elapsedTime >= duration * 60)
        {
            StopTimer();
            Countdown.countdownText.gameObject.SetActive(true);
            
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        Countdown.countdownText.text = "Finishin in...";
        isRunning = false;
        Countdown.StartCountdown();
        
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}