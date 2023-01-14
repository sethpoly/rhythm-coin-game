using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float currentTime = 0;
    public float endTime = 3;
    private bool timerRunning = false;
    public TextMeshProUGUI timeText;

    public void StartTimer(int until)
    {
        // Reset time
        currentTime = 0;
        endTime = until;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    void Update()
    {
        if(timerRunning) 
        {
            if (currentTime < endTime)
            {
                currentTime += Time.time;
                DisplayTime(currentTime);
            } else 
            {
                timerRunning = false;
                currentTime = endTime;
            }
        }
    }

    private void DisplayTime(float currentTime)
    {
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float milliSeconds = (currentTime % 1) * 1000;
        timeText.text = string.Format("{0:00}:{1:00}", seconds, milliSeconds);
    }

    public float GetCurrentTime() {
        return currentTime;
    }
}
