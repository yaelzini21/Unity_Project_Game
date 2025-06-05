using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject guidePanel;
    public GameController gameController;
    public MenuController menuController;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameController.started)
        {
            if (menuController.isGuideOpen)
            {
                // אם אנחנו במסך מדריך - לצאת חזרה לפאנל הקודם (pause/menu)
                menuController.BackFromGuide();
            }
            else
            {
                if (!isPaused)
                    PauseGame();
                else
                    ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        menuController.isGuideOpen = false;
    }

    public void ShowGuide()
    {
        menuController.ShowGuideFromPause();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
