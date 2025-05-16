using UnityEngine;
using UnityEngine.UI;

public class VictoryMenuHandler : MonoBehaviour
{
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(BackToMainMenu);
    }


    public void RestartGame()
    {
        gameObject.SetActive(false);
        GameHandler.Instance.Restart();
    }

    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
        GameHandler.Instance.ToMenu();
    }

}

