using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public bool gamePaused = false;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject nextSceenScreen;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject continuePanel;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private TMP_Dropdown dropdown;
    public string selectedProfile = "";
    public Difficulty selectedDifficulty = Difficulty.Normal;
    List<SaveData> saveDatas;

    public void Start()
    {
        RefreshSaveDataOptions();
    }
    
    private void RefreshSaveDataOptions()
    {
        if (dropdown == null || continueButton == null || continuePanel == null) return;
        saveDatas = SaveManager.LoadAllProfiles();
        dropdown.ClearOptions();
        if (saveDatas.Count > 0)
        {
            selectedProfile = saveDatas[0].profileName;
            TMP_Dropdown.OptionDataList optionData = new TMP_Dropdown.OptionDataList();
            foreach (SaveData saveData in saveDatas)
            {
                optionData.options.Add(new TMP_Dropdown.OptionData(saveData.profileName));
            }

            dropdown.AddOptions(optionData.options);
            continueButton.SetActive(true); 
        }
        else
        {
            continueButton.SetActive(false);
            continuePanel.SetActive(false);
        }
    }
    
    #region Main Mennu
    public void StartGame()
    {
        CreateAndLaunchNewProfile();
    }

    public void ContinueGame()
    {
        ContinueGameOnSelectedProfile();
    }
    public void SelectDifficulty(int difficulty)
    {
        selectedDifficulty = (Difficulty)difficulty;
    }
    public void SelectProfile(string profileName)
    {
        selectedProfile = profileName;
    }
    public void SelectProfile(int idx)
    {
        selectedProfile = saveDatas[idx].profileName;
    }
    public void CreateAndLaunchNewProfile()
    {
        SaveData saveData = new SaveData();
        saveData.profileName = selectedProfile;
        saveData.difficulty = selectedDifficulty;
        SaveManager.Save(saveData);
        SceneManager.LoadScene("Level1.1");
    }

    public void ContinueGameOnSelectedProfile()
    {
        SaveData saveData = SaveManager.Load(selectedProfile);
        int sceneIndex = saveData.currentSceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void DeleteSelectedProfile()
    {
        SaveManager.DeleteData(selectedProfile);
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
        if (string.IsNullOrEmpty(saveData.lastSaveSpotID)) SceneManager.LoadScene("Main_Menu");
        int sceneIndex = saveData.currentSceneIndex;
        SceneManager.LoadScene(sceneIndex);
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
