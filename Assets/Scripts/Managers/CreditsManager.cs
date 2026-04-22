using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuManager.gamePaused)
            {
                menuManager.ResumeGame();
            } else
            {
                menuManager.PauseGame();
            }
        }
    }
}
