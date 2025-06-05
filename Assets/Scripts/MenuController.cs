using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject guidePanel;
    public GameObject pausePanel;
    public GameController gameController;
    public BallController ballController;
    public GameObject menuTitle;

    private GameObject lastOpenedPanel;
    public bool isGuideOpen = false;
    public GameObject instructionsTextObject;
    public GameObject difficultyPanel; 
       
       

    // public Color guideColorFromMenu = new Color(0.184f, 0.278f, 0.773f); //2F47C5
    // public Color guideColorFromPause = new Color(0.41f, 0.51f, 0.78f); // #6981C8

      
    void Start()
    {
        menuTitle.SetActive(true);
        ShowMenu();
        ballController.MenuGo();
        lastOpenedPanel = menuPanel;
    }

    public void OnPlayVsPlayerClicked()
    {
        menuPanel.SetActive(false);
        menuTitle.SetActive(false);
        ballController.MenuStop();
        gameController.StartGame(isVsAI: false,"medium");
    }

    public void OnPlayVsAIClicked()
    {
        menuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        menuTitle.SetActive(true);
    }

    public void OnSelectEasy()
    {
        difficultyPanel.SetActive(false);
        menuTitle.SetActive(false);
        ballController.MenuStop();
        gameController.StartGame(isVsAI: true,"easy");
        
    }

    public void OnSelectMedium()
    {
        difficultyPanel.SetActive(false);
        menuTitle.SetActive(false);
        ballController.MenuStop();
        gameController.StartGame(isVsAI: true, "medium");
        
    }

    public void OnSelectHard()
    {
        difficultyPanel.SetActive(false);
        menuTitle.SetActive(false);
        ballController.MenuStop();
        gameController.StartGame(isVsAI: true, "hard");
        
    }


    public void OnBackFromDifficulty()
    {
        difficultyPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void ShowGuideFromMenu()
    {
        lastOpenedPanel = menuPanel;
        guidePanel.SetActive(true);
        menuPanel.SetActive(false);
        menuTitle.SetActive(false);
        isGuideOpen = true;
        gameController.isGuideMode = true;
        instructionsTextObject.SetActive(true);
        gameController.enableAllRackets();
        Debug.Log("ShowGuideFromMenu called");
    }

    public void ShowGuideFromPause()
    {
        lastOpenedPanel = pausePanel;
        guidePanel.SetActive(true);
        pausePanel.SetActive(false);
        isGuideOpen = true;
        instructionsTextObject.SetActive(false);

        Debug.Log("ShowGuideFromPause called");

    }

    public void BackFromGuide()
    {
        guidePanel.SetActive(false);

        isGuideOpen = false;

        Debug.Log("BackFromGuide called");
        Debug.Log("lastOpenedPanel: " + lastOpenedPanel.name);

        if (lastOpenedPanel == menuPanel)
        {
            menuPanel.SetActive(true);
            gameController.startOnlyAiGame();
        }
        else if (lastOpenedPanel == pausePanel)
        {
            pausePanel.SetActive(true);
        }

        menuTitle.SetActive(true);
        lastOpenedPanel = menuPanel;
        gameController.isGuideMode = false;

    }

    public void ShowMenu()
    {
        guidePanel.SetActive(false);
        pausePanel.SetActive(false);
        menuPanel.SetActive(true);
        menuTitle.SetActive(true);
    }
}
