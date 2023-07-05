using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public float countdownTime = 6f; // Time in seconds for the countdown
    public TextMeshProUGUI countdownText; // Reference to the UI text element

    private float currentTime = 0f;
    private bool isCountdownStarted = false;

    public TimeController TimeController;
    private Manager Manager;


    private void Awake()
    {
        countdownText.text = currentTime.ToString("Prepare to Start");
        Manager = FindObjectOfType<Manager>();
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

            if ( currentTime <= 5f ) { countdownText.text = currentTime.ToString("0"); }
            
            if (currentTime < 1 && !TimeController.isFinished) { countdownText.text = "Start!"; }

            if (currentTime <= 0 && !TimeController.isFinished)
            {
                StopCountdown();
                TimeController.StartTimer();
            }
             
            if (currentTime <= 0 && TimeController.isFinished)
            {
                StopCountdown();
                if (Manager.Scenes.Count > 0)
                {
                    Manager.Shuffle();
                    Manager.LoadScene();
                }
               // else
                  //  SceneManager.LoadScene(0);
            }
        }

        if (!isCountdownStarted && TimeController.isFinished)
        {
            StartCountdown();
        }
    }

    public void StartCountdown()
    {
        ResetCountdown();
        isCountdownStarted = true;
        
    }

    public void StopCountdown()
    {
        isCountdownStarted = false;
        currentTime = 0;
        countdownText.gameObject.SetActive(false);
    }

    public void ResetCountdown()
    {
        currentTime = countdownTime;
    }
}
