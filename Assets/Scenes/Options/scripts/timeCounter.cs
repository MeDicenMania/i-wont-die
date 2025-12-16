using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    public TMP_Text timerText;

    public float timeElapsed = 0f;
    public bool running = false;

    void Update()
    {
        if (!running) return;

        timeElapsed += Time.deltaTime;
        UpdateDisplay();
    }

    public void StartTimer()
    {
        timeElapsed = 0f;
        running = true;
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
    }

    public void StopTimer()
    {
        running = false;
    }

    void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}

