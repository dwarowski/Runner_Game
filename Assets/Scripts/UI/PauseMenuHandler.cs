using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject pauseMenu; // Панель паузы
    public Button resumeButton;
    public Button mainMenuButton;
    [HideInInspector]
    public GameObject gameMenu;
    [HideInInspector]
    public GameObject mainMenu;

    private bool isPaused = false;

    void Start()
    {
        if (gameMenu == null)
        {
            Debug.LogWarning("Game canvas not set, check GameStartHandler script");
        }
        pauseMenu.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        gameMenu.SetActive(false);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gameMenu.SetActive(true);
        isPaused = false;
    }

    public void BackToMainMenu()
    {
        pauseMenu.SetActive(false);
        gameMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
