using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VictoryDisplay : MonoBehaviour
{
    public GameObject victoryPanel;

    public TextMeshProUGUI pointsText;  
    public TextMeshProUGUI timeText;  
    public TextMeshProUGUI attText; 
    public TextMeshProUGUI rankText; 

    [Range(0, 1)] public float weightPoints = 0.2f;
    [Range(0, 1)] public float weightTime = 0.8f;
    public float deathPen = 0.25f;

    public void calculate(float playerTime, int playerPoints, int playerDeaths, int maxPoints, float timeGoal)
    {
        Time.timeScale = 0f;

        if (victoryPanel != null) victoryPanel.SetActive(true);

        float pointsScore = (maxPoints > 0) ? ((float)playerPoints / maxPoints * 100f) : 100f;
        float timeScore = (playerTime <= timeGoal) ? 100f : (timeGoal / playerTime) * 100f;

        float baseScore = (pointsScore * weightPoints) + (timeScore * weightTime);
        float discount = playerDeaths * deathPen;

        float finalRank = Mathf.Max(0, baseScore - discount);

        StartCoroutine(AnimarEstadisticas(playerPoints, playerTime, playerDeaths, finalRank));
    }

    IEnumerator AnimarEstadisticas(int finalPoints, float finalTime, int finalDeath, float rank)
    {
        rankText.text = "";
        if (pointsText) pointsText.text = "0";
        if (timeText) timeText.text = "00:00";
        if (attText) attText.text = "0";

        float duration = 2.0f;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            if (pointsText != null)
            {
                float pts = Mathf.Lerp(0, finalPoints, t);
                pointsText.text = pts.ToString("F0");
            }

            if (timeText != null)
            {
                float time = Mathf.Lerp(0, finalTime, t);
                timeText.text = timeFormat(time);
            }

            if (attText != null)
            {
                float deaths = Mathf.Lerp(0, finalDeath, t);
                attText.text = deaths.ToString("F0");
            }

            yield return null;
        }

        if (pointsText) pointsText.text = finalPoints.ToString();
        if (timeText) timeText.text = timeFormat(finalTime);
        if (attText) attText.text = finalDeath.ToString();

        showLetter(rank);

        var boton = victoryPanel.GetComponentInChildren<Button>();
        if (boton) EventSystem.current.SetSelectedGameObject(boton.gameObject);
    }

    // Función auxiliar para convertir segundos (80.5) a reloj (01:20)
    string timeFormat(float tiempoEnSegundos)
    {
        int min = Mathf.FloorToInt(tiempoEnSegundos / 60f);
        int sec = Mathf.FloorToInt(tiempoEnSegundos % 60f);
        return $"{min:00}:{sec:00}";
    }

    void showLetter(float porcentaje)
    {
        string leter = porcentaje switch
        {
            >= 100f => "S+",
            >= 90f => "S",
            >= 80f => "A",
            >= 70f => "B",
            >= 60f => "C",
            >= 50f => "D",
            >= 40f => "E",
            _ => "F"
        };

        rankText.color = leter switch
        {
            "S+" or "S" => new Color(1f, 0.84f, 0f),
            "A" or "B" => Color.green,
            "C" => Color.cyan,
            "D" or "E" => new Color(1f, 0.5f, 0f),
            _ => Color.red
        };

        rankText.text = leter;

        StartCoroutine(PopLeter());
    }

    IEnumerator PopLeter()
    {
        float t = 0;
        rankText.transform.localScale = Vector3.zero;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 5;
            rankText.transform.localScale = Vector3.one * t;
            yield return null;
        }
    }

    public void HidePanel()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (pointsText) pointsText.text = "0";
        if (timeText) timeText.text = "00:00";
        if (attText) attText.text = "0";
    }
}