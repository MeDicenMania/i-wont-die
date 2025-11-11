using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject gameplayUI;
    bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        paused = !paused;
        pauseUI.SetActive(paused);
        gameplayUI.SetActive(!paused);
        Time.timeScale = paused ? 0f : 1f;
    }

}
