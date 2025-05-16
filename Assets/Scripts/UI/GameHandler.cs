using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }
    public GameObject mainMenuCanvas;
    public GameObject gameMenuCanvas;
    public GameObject finishMenuCanvas;
    public GameObject pauseMenuObject;
    public GameObject gameOverCanvas;
    public GameObject mainMenuLocation;
    public GameObject carPrefab;            // Префаб первой машины
    public GameObject surfaceCollector;
    public GameObject roadCollector;
    private GameObject currentCar;          // Текущая инстанс-машина

    public Transform carHolder;         // Точка, где должна появиться машина
    public UI ui;                           // UI скрипт со спидометром и пр.
    public MusicPlayer musicPlayerScript;

    private Button startButton;
    private Button pauseButton;
    private Button restartButton;
    private Button menuButton;
    private Transform audioPlayer;
    private GameObject pauseMenuCanvas;
    private PauseMenuHandler pauseMenuScript;
    private Transform cameraAnchor;
    private CarControl carControl;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        InitComponents();
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        ClearScene();
        // Отключаем меню
        mainMenuCanvas.SetActive(false);

        // Включаем игровой UI
        gameMenuCanvas.SetActive(true);

        // Спавним первую машину
        currentCar = Instantiate(carPrefab, carHolder.position, carHolder.rotation, carHolder);
        Transform carTrasnform = currentCar.GetComponent<Transform>();


        // Привязываем камеру к якорю на новой машине
        Transform cameraAnchor = currentCar.transform.Find("CameraAnchor");
        if (cameraAnchor != null)
        {
            Camera.main.transform.SetParent(cameraAnchor);
            Camera.main.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("CameraAnchor not found");
        }

        // Подключаем UI к машине
        currentCar.TryGetComponent(out carControl);
        if (carControl != null && ui != null)
        {
            carControl.ui = ui;
        }

        if (roadCollector.TryGetComponent<RoadGenerator>(out var roadGen))
        {
            roadGen.Initialize(carTrasnform);
        }

        if (surfaceCollector.TryGetComponent<GroundRepeater>(out var surfaceGen))
        {
            surfaceGen.Initialize(carTrasnform);
        }

        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        pauseMenuScript.PauseGame();
    }

    public void GameOver()
    {
        gameMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.transform.Find("Total Score").TryGetComponent<TextMeshProUGUI>(out var totalScoreUGUI);
        totalScoreUGUI.text = Math.Round(carControl.GetTotalDistance()).ToString();
        gameOverCanvas.SetActive(true);
    }

    public void Restart()
    {
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        this.StartGame();
    }

    public void ToMenu()
    {
        ClearScene();

        gameMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        musicPlayerScript.audioPlayer = audioPlayer;

        mainMenuLocation.transform.Find("CameraAnchor").TryGetComponent(out cameraAnchor);
        if (cameraAnchor != null)
        {
            Camera.main.transform.SetParent(cameraAnchor);
            Camera.main.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    public void ShowVictory()
    {

        finishMenuCanvas.transform.Find("Total Score").TryGetComponent<TextMeshProUGUI>(out var totalScoreUGUI);
        totalScoreUGUI.text = Math.Round(carControl.GetTotalDistance()).ToString();

        Time.timeScale = 0f;
        finishMenuCanvas.SetActive(true);
        gameMenuCanvas.SetActive(false);
    }

    private void ClearScene()
    {
        carHolder.gameObject.TryGetComponent<CarEvolutionHandler>(out var carEvolutionHandler);
        carEvolutionHandler.currentStage = 0;

        if (roadCollector.transform.childCount > 0)
        {
            foreach (Transform child in roadCollector.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (surfaceCollector.transform.childCount > 0)
        {
            foreach (Transform child in surfaceCollector.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (carHolder.childCount > 0)
        {
            foreach (Transform child in carHolder)
            {
                Destroy(child.gameObject);
            }
        }

    }

    private void InitComponents()
    {
        pauseMenuCanvas = pauseMenuObject.transform.Find("Pause Menu Canvas").gameObject;

        mainMenuCanvas.transform.Find("Audio Player").TryGetComponent(out audioPlayer);
        musicPlayerScript.audioPlayer = audioPlayer;

        pauseMenuObject.TryGetComponent(out pauseMenuScript);
        pauseMenuScript.SetPrivateVars(gameMenuCanvas, musicPlayerScript);

        // Searching buttons in main menu
        Transform mainMenuButtons = mainMenuCanvas.transform.Find("Buttons");
        if (mainMenuButtons == null)
        {
            Debug.LogError("Buttons GameObject not found on the mainMenuCanvas. Make sure it exists and is named correctly.");
            return; // Stop if buttons is missing
        }
        mainMenuButtons.transform.Find("Start Button").TryGetComponent(out startButton);
        startButton.onClick.AddListener(StartGame);

        // Searching buttons in main menu
        Transform gameOverButtons = gameOverCanvas.transform.Find("Buttons");
        if (gameOverButtons == null)
        {
            Debug.LogError("Buttons GameObject not found on the mainMenuCanvas. Make sure it exists and is named correctly.");
            return; // Stop if buttons is missing
        }
        gameOverButtons.transform.Find("Restart Button").TryGetComponent(out restartButton);
        gameOverButtons.transform.Find("Menu Button").TryGetComponent(out menuButton);
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(ToMenu);

        gameMenuCanvas.transform.Find("Pause Button").TryGetComponent(out pauseButton);
        pauseButton.onClick.AddListener(PauseGame);
    }
}
