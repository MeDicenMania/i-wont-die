using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsCounter : MonoBehaviour
{
    public int points = 0;
    public TMP_Text pointsText;

    public static PointsCounter Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdatePointsText();
    }

    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsText();
    }

    void UpdatePointsText()
    {
        pointsText.text = points.ToString();
    }

    public void ResetPoints()
    {
        points = 0;
        UpdatePointsText();
    }


}