using UnityEngine;
using UnityEngine.SceneManagement;
public class scenemanager : MonoBehaviour
{

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LevelSelection()
    {
        SceneManager.LoadScene("Select Level");
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene("Options");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}