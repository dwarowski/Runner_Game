using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenuHandler : MonoBehaviour
{
    public GameObject victoryMenu;
    public Button restartButton;
    public Button mainMenuButton;

    private GameObject gameMenu;
    private GameObject mainMenu;

    void Start()
    {
        victoryMenu.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    public void ShowVictory()
    {
        Time.timeScale = 0f;
        victoryMenu.SetActive(true);
        if (gameMenu != null)
            gameMenu.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SetGameAndMainMenu(GameObject gameMenu, GameObject mainMenu)
    {
        this.gameMenu = gameMenu;
        this.mainMenu = mainMenu;
    }
}

