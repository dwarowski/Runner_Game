using UnityEngine;
using UnityEngine.UI;

public class GameStartHandler : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public Button playButton;
    public Button pauseButton;

    public GameObject carPrefab;            // Префаб первой машины
    public Transform carSpawnPoint;         // Точка, где должна появиться машина

    public Camera mainCamera;               // Главная камера (Main Camera)
    public UI ui;                           // UI скрипт со спидометром и пр.

    public GameObject surfaceCollector;
    public GameObject roadCollector;

    private GameObject currentCar;          // Текущая инстанс-машина


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
            carSpawnPoint == null)
        {
            Debug.LogWarning("GameStartHandler: не все поля заданы в инспекторе.");
            return;
        }

        PauseMenuHandler pauseMenuScript = pauseMenu.GetComponent<PauseMenuHandler>();
        pauseMenuScript.gameMenu = gameMenu;
        pauseMenuScript.mainMenu = mainMenu;

        playButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(PauseGame);

        Time.timeScale = 0f;

        mainCamera.gameObject.SetActive(true);
        gameMenu.SetActive(false);
    }

    public void StartGame()
    {
        // Отключаем меню
        mainMenu.SetActive(false);

        // Включаем игровой UI
        gameMenu.SetActive(true);

        // Спавним первую машину
        currentCar = Instantiate(carPrefab, carSpawnPoint.position, carSpawnPoint.rotation, carSpawnPoint);
        Transform carTrasnform = currentCar.GetComponent<Transform>();


        // Привязываем камеру к якорю на новой машине
        Transform cameraAnchor = currentCar.transform.Find("CameraAnchor");
        if (cameraAnchor != null)
        {
            mainCamera.transform.SetParent(cameraAnchor);
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.localRotation = Quaternion.identity;
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

        RoadGenerator roadGen = roadCollector.GetComponent<RoadGenerator>();
        if (roadGen != null)
        {
            roadGen.player = carTrasnform;
            roadGen.Initialize(carTrasnform);
        }

        GroundRepeater surfaceGen = surfaceCollector.GetComponent<GroundRepeater>();
        if (surfaceGen != null)
        {
            surfaceGen.player = carTrasnform;
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
