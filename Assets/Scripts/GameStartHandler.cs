using UnityEngine;
using UnityEngine.UI;

public class GameStartHandler : MonoBehaviour
{
    public Camera mainMenuCamera;
    public Camera gameCamera;
    public Canvas mainMenuCanvas;
    public Canvas gameCanvas;
    public Button playButton; // Assign the Play Button from the Main Menu

    void Start()
    {
        // Make sure the references are assigned correctly, otherwise show a warning.
        if (mainMenuCamera == null || gameCamera == null || mainMenuCanvas == null || gameCanvas == null || playButton == null)
        {
            Debug.LogWarning("Some of the references in GameStartHandler are not set.  Please check the Inspector.");
        }

        //Optional:  If you are using a button to start, this can be in Start.
        playButton.onClick.AddListener(StartGame);

        Time.timeScale = 0f;
    }



    public void StartGame()
    {
        // Disable the Main Menu
        mainMenuCamera.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(false);

        // Enable the Game
        gameCamera.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(true);

        //Optional:
        //Disable the button after the game starts.
        playButton.interactable = false;

        Time.timeScale = 1f;
        // Add any other game initialization logic here, such as starting the game manager, etc.
        Debug.Log("Game started!");
    }
}
