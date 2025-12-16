using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject gameUI;
    private GameObject pauseUI;
    private GameObject victoryUI;

    private PointsCounter pointCounter;
    private DeathCounter deathCounter;
    private TimeCounter timeCounter;

    public bool paused = false;
    private bool isGameLevel = false;
    private bool levelCompleted = false; // Prevent multiple triggers
    public bool LevelCompleted => levelCompleted; // reed only

    private string sceneToLoad = "";

    public enum ConfirmAction
    {
        None,
        RestartLevel,
        GoToMenu,
        ExitGame
    }

    public GameObject pauseConfirmationPanel; 
    public GameObject victoryConfirmationPanel;
    private GameObject activeConfirmationPanel = null;
    private ConfirmAction pendingAction = ConfirmAction.None;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (SceneManager.GetActiveScene().name == "GameManager")
                SceneManager.LoadScene("Main Menu");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (isGameLevel && Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeConfirmationPanel != null && activeConfirmationPanel.activeSelf)
            {
                Cancel();
                return;
            }
            TogglePause();
        }
    }


    // -------------------------
    // Scene Management
    // -------------------------

    public void LoadScene(string targetScene)
    {
        string current = SceneManager.GetActiveScene().name;
        bool comingFromMenu = current == "Main Menu";

        if (comingFromMenu || targetScene == "Main Menu")
        {
            // Load directly if coming from menu or next scene is Main Menu
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            // Use LoadingScene for other levels
            sceneToLoad = targetScene;
            SceneManager.LoadScene("LoadingScene");
        }
    }

    public string ConsumeSceneToLoad()
    {
        string temp = sceneToLoad;
        sceneToLoad = "";
        return temp;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject uiPanels = GameObject.Find("GameManager");

        if (uiPanels != null)
        {
            gameUI = uiPanels.transform.Find("GameUI")?.gameObject;
            pauseUI = uiPanels.transform.Find("PauseUI")?.gameObject;
            victoryUI = uiPanels.transform.Find("VictoryUI")?.gameObject;
        }

        if (pauseUI != null) pauseUI.SetActive(false);
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
            var displayScript = victoryUI.GetComponent<VictoryDisplay>();
            if (displayScript != null)
            {
                displayScript.HidePanel();
            }

        }


        isGameLevel = scene.name.StartsWith("Level ");

        if (gameUI != null)
        {
            gameUI.SetActive(isGameLevel);

            pointCounter = gameUI.GetComponentInChildren<PointsCounter>(true);
            deathCounter = gameUI.GetComponentInChildren<DeathCounter>(true);
            timeCounter = gameUI.GetComponentInChildren<TimeCounter>(true);

            if (timeCounter != null)
                timeCounter.StartTimer();
        }

        paused = false;
        Time.timeScale = 1f;

        if (activeConfirmationPanel != null)
            activeConfirmationPanel.SetActive(false);
        
        activeConfirmationPanel = null;
        pendingAction = ConfirmAction.None;

        levelCompleted = false; // Reset for new level
    }

    // -------------------------
    // MENU
    // -------------------------

    public void LoadMainMenu()
    {
        ResetCounters();
        LoadScene("Main Menu");
    }

    public void LoadLevelSelection() => LoadScene("Select Level");
    public void LoadOptions() => LoadScene("Options");
    public void InGameOptions() => LoadScene("inGameOptions");
    public void LoadInGameOptions() => LoadScene("gameOptions");
    public void QuitGame() => Application.Quit();

    public void Button_MainMenu() => LoadMainMenu();
    public void Button_Options() => LoadOptions();
    public void Button_LevelSelect() => LoadLevelSelection();
    public void Button_LoadLevel(string levelName) => LoadScene(levelName);
    public void Button_InGOptions() => InGameOptions();
    public void Button_Restart() => RestartLevel();
    public void Button_Quit() => QuitGame();

    // -------------------------
    // GAME MANAGEMENT
    // -------------------------

    public void TogglePause()
    {
        if (pauseUI == null) return;

        paused = !paused;
        pauseUI.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;

        if (paused)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (pauseUI.transform.childCount > 0)
                EventSystem.current.SetSelectedGameObject(pauseUI.transform.GetChild(0).gameObject);
        }
    }

    public void RestartLevel()
    {
        paused = false;
        Time.timeScale = 1f;

        string current = SceneManager.GetActiveScene().name;
        LoadScene(current);

        ResetCounters();
    }

    public void ResetCounters()
    {
        if (pointCounter != null) pointCounter.ResetPoints();
        if (deathCounter != null) deathCounter.ResetDeaths();
        if (timeCounter != null) timeCounter.ResetTimer();
    }

    // -------------------------
    // LEVEL COMPLETION
    // -------------------------

    public void OnLevelCompleted()
    {
        if (levelCompleted) return;
        levelCompleted = true;

        if (timeCounter != null) timeCounter.StopTimer();

        if (gameUI != null) gameUI.SetActive(false);

    }


    public void Button_Continue()
    {
        Time.timeScale = 1f;

        string nextScene;
        Scene current = SceneManager.GetActiveScene();
        string name = current.name;

        if (name.StartsWith("Level ") && int.TryParse(name.Substring(6), out int number))
        {
            nextScene = (number < 20) ? "Level " + (number + 1) : "Main Menu";
        }
        else
        {
            nextScene = "Main Menu";
        }
        ResetCounters();
        LoadScene(nextScene);
    }

    // -------------------------
    // CONFIRMATION PANEL
    // -------------------------

    // PauseUI
    public void RequestRestartPause() => AskConfirmation(ConfirmAction.RestartLevel, pauseConfirmationPanel);
    public void RequestGoToMenuPause() => AskConfirmation(ConfirmAction.GoToMenu, pauseConfirmationPanel);
    public void RequestExitGamePause() => AskConfirmation(ConfirmAction.ExitGame, pauseConfirmationPanel);

    // VictoryUI
    public void RequestRestartVictory() => AskConfirmation(ConfirmAction.RestartLevel, victoryConfirmationPanel);
    public void RequestGoToMenuVictory() => AskConfirmation(ConfirmAction.GoToMenu, victoryConfirmationPanel);
    public void RequestExitGameVictory() => AskConfirmation(ConfirmAction.ExitGame, victoryConfirmationPanel);

    private void AskConfirmation(ConfirmAction action, GameObject panel)
    {
        pendingAction = action;

        if (panel != null)
        {
            panel.SetActive(true);
            activeConfirmationPanel = panel;
        }

        Time.timeScale = 0f;
    }


public void Confirm()
{
    switch (pendingAction)
    {
        case ConfirmAction.RestartLevel:
            RestartLevel();
            break;
        case ConfirmAction.GoToMenu:
            LoadMainMenu();
            break;
        case ConfirmAction.ExitGame:
            QuitGame();
            break;
    }

    pendingAction = ConfirmAction.None;

    if (activeConfirmationPanel != null)
        activeConfirmationPanel.SetActive(false);

    activeConfirmationPanel = null;
}

public void Cancel()
{
    pendingAction = ConfirmAction.None;

    if (activeConfirmationPanel != null)
        activeConfirmationPanel.SetActive(false);

    activeConfirmationPanel = null;
}

}
