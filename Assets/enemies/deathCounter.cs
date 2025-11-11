using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    public int deaths = 0;
    public TMP_Text deathText;

    void Start()
    {
        UpdateDeathsText();
    }

    public void AddDeaths(int amount)
    {
        deaths += amount;
        UpdateDeathsText();
    }

    void UpdateDeathsText()
    {
        deathText.text = deaths.ToString();
    }
}