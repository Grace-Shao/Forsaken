using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void StartGame()
    {
        SaveManager.DeleteData();
        SceneManager.LoadScene("Level1");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
        //for debug ^^
    }
}
