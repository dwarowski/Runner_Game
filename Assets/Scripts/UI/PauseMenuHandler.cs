using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    public Button resumeButton;
    public Button mainMenuButton;
    public Transform audioPlayer;
    public GameObject pauseMenu; // Панель паузы

    private GameObject gameMenuCanvas;
    private MusicPlayer musicPlayerScript;
    private bool isPaused = false;

    void Start()
    {
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
        gameMenuCanvas.SetActive(false);
        musicPlayerScript.audioPlayer = audioPlayer;
        musicPlayerScript.InitComponents();
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gameMenuCanvas.SetActive(true);
        isPaused = false;
    }

    public void BackToMainMenu()
    {
        pauseMenu.SetActive(false);
        GameHandler.Instance.ToMenu();

    }


    public void SetPrivateVars(GameObject gameMenu, MusicPlayer musicPlayerScript)
    {
        this.gameMenuCanvas = gameMenu;
        this.musicPlayerScript = musicPlayerScript;
    }
}
