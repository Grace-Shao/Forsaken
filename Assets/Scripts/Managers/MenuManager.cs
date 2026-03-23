using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool gamePaused = false;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject nextSceenScreen;
    [SerializeField] private GameObject hud;
    
    #region Main Mennu
    public void StartGame()
    {
        SaveManager.DeleteData(); //swap out later
        SceneManager.LoadScene("Level1.1");
    }

    public void ContinueGame()
    {
        SaveData saveData = SaveManager.Load();
        int sceneIndex = saveData.currentSceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
        //for debug ^^
    }
    #endregion

    #region Pause Menu
    // Pause game
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f;
        gamePaused = true;
    }
    // Resume game
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f;
        gamePaused = false;
    }
    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // Options (not added yet)
    public void LoadOptions()
    {
        
    }
    public void LoadMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        gamePaused = false;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Main_Menu");
    }
    #endregion

    #region Win/Lose Screens
    public void TryAgain()
    {
        SaveData saveData = SaveManager.Load();
        if (string.IsNullOrEmpty(saveData.lastSaveSpotID)) RestartGame();
        int sceneIndex = saveData.currentSceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }
    
    public void RestartGame()
    {
        SaveManager.DeleteData();
        SceneManager.LoadScene("Level1.1");
    }
    #endregion

    #region Transitions
    public void OpenSceneProgression()
    {
        gameManager.DisablePlayer();
        nextSceenScreen.SetActive(true);
    }
    public void CloseSceneProgression()
    {
        gameManager.EnablePlayer();
        nextSceenScreen.SetActive(false);
    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion

}
