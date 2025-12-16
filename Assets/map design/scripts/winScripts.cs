using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private GameManager gameManager;

    private PointsCounter pointsCounter;
    private TimeCounter timeCounter;
    private DeathCounter deathCounter;
    private VictoryDisplay victoryDisplay;

    public int maxPoints = 100;
    public float timeGoal = 60f;

    private void Start()
    {
        gameManager = GameManager.Instance;

        pointsCounter = FindFirstObjectByType<PointsCounter>();
        timeCounter = FindFirstObjectByType<TimeCounter>();
        deathCounter = FindFirstObjectByType<DeathCounter>();
        victoryDisplay = FindFirstObjectByType<VictoryDisplay>(FindObjectsInactive.Include);  // en caso que por alguna razon este apagado

        if (victoryDisplay == null) Debug.LogError("¡Falta el VictoryDisplay en la escena!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (gameManager != null && !gameManager.LevelCompleted)
        {
            gameManager.OnLevelCompleted();

            if (victoryDisplay != null)
            {

                int finalPoints = pointsCounter.points;
                float finalTime = timeCounter.timeElapsed;
                int finalDeaths = deathCounter.deaths;

                victoryDisplay.calculate(finalTime, finalPoints, finalDeaths, maxPoints, timeGoal);
            }
            GetComponent<Collider2D>().enabled = false;
        }
    }
}