using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void StartGame()
    {
        SceneManager.LoadScene("M1 Demo");


    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
        //for debug ^^
    }
}
