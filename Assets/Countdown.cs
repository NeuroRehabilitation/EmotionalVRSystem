using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public float countdownTime = 5f; // Time in seconds for the countdown
    public TextMeshProUGUI countdownText; // Reference to the UI text element

    private float currentTime = 0f;
    private bool isCountdownStarted = false;

    public TimeController TimeController;


    private void Awake()
    {
        countdownText.text = currentTime.ToString("Prepare to Start");
    }
    private void Start()
    {
        currentTime = countdownTime;
    }

    private void Update()
    {
        if (isCountdownStarted)
        {
            currentTime -= Time.deltaTime;
            countdownText.text = currentTime.ToString("0");

            if (currentTime < 1)
            {
                countdownText.text = "Start!";
                
            }

            if (currentTime <= 0)
            {
                StopCountdown();
                TimeController.StartTimer();
            }
        }
    }

    public void StartCountdown()
    {
        isCountdownStarted = true;
        currentTime = countdownTime;
    }

    public void StopCountdown()
    {
        isCountdownStarted = false;
        currentTime = 0;
        countdownText.gameObject.SetActive(false);
    }

    public void ResetCountdown()
    {
        countdownTime = 5f;
    }
}
