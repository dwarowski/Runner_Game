using UnityEngine;
using UnityEngine.UI;

public class GameStartHandler : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject carPrefab;            // Префаб первой машины
    public GameObject surfaceCollector;
    public GameObject roadCollector;
    public GameObject mainCamera;               // Главная камера (Main Camera)
    private GameObject currentCar;          // Текущая инстанс-машина

    public Button playButton;
    public Button pauseButton;

    public Transform carHolder;         // Точка, где должна появиться машина
    public UI ui;                           // UI скрипт со спидометром и пр.

    private PauseMenuHandler pauseMenuScript;
    void Start()
    {
        if (ui == null ||
            mainCamera == null ||
            mainMenu == null ||
            surfaceCollector == null || 
            roadCollector == null || 
            gameMenu == null || 
            playButton == null || 
            pauseButton == null || 
            pauseMenu == null || 
            carPrefab == null || 
            carHolder == null)
        {
            Debug.LogWarning("GameStartHandler: не все поля заданы в инспекторе.");
            return;
        }
        pauseMenuScript = pauseMenu.GetComponent<PauseMenuHandler>();

        pauseMenuScript.SetGameAndMainMenu(gameMenu, mainMenu);
        playButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(PauseGame);

        Time.timeScale = 0f;

        mainCamera.SetActive(true);
        gameMenu.SetActive(false);
    }

    public void StartGame()
    {
        // Отключаем меню
        mainMenu.SetActive(false);

        // Включаем игровой UI
        gameMenu.SetActive(true);

        // Спавним первую машину
        currentCar = Instantiate(carPrefab, carHolder.position, carHolder.rotation, carHolder);
        Transform carTrasnform = currentCar.GetComponent<Transform>();


        // Привязываем камеру к якорю на новой машине
        Transform cameraAnchor = currentCar.transform.Find("CameraAnchor");
        if (cameraAnchor != null)
        {
            mainCamera.transform.SetParent(cameraAnchor);
            mainCamera.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("CameraAnchor не найден в машине.");
        }

        // Подключаем UI к машине
        CarControl carControl = currentCar.GetComponent<CarControl>();
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
        Debug.Log("Game started и машина создана!");
    }

    public void PauseGame()
    {
        PauseMenuHandler pauseMenuScript = pauseMenu.GetComponent<PauseMenuHandler>();
        pauseMenuScript.PauseGame();
    }
}
