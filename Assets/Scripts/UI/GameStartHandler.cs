using UnityEngine;
using UnityEngine.UI;

public class GameStartHandler : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public Button playButton;
    public Button pauseButton;

    public GameObject carPrefab;            // ������ ������ ������
    public Transform carSpawnPoint;         // �����, ��� ������ ��������� ������

    public Camera mainCamera;               // ������� ������ (Main Camera)
    public UI ui;                           // UI ������ �� ����������� � ��.

    public GameObject surfaceCollector;
    public GameObject roadCollector;

    private GameObject currentCar;          // ������� �������-������


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
            Debug.LogWarning("GameStartHandler: �� ��� ���� ������ � ����������.");
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
        // ��������� ����
        mainMenu.SetActive(false);

        // �������� ������� UI
        gameMenu.SetActive(true);

        // ������� ������ ������
        currentCar = Instantiate(carPrefab, carSpawnPoint.position, carSpawnPoint.rotation, carSpawnPoint);
        Transform carTrasnform = currentCar.GetComponent<Transform>();


        // ����������� ������ � ����� �� ����� ������
        Transform cameraAnchor = currentCar.transform.Find("CameraAnchor");
        if (cameraAnchor != null)
        {
            mainCamera.transform.SetParent(cameraAnchor);
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("CameraAnchor �� ������ � ������.");
        }

        // ���������� UI � ������
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
        Debug.Log("Game started � ������ �������!");
    }

    public void PauseGame()
    {
        PauseMenuHandler pauseMenuScript = pauseMenu.GetComponent<PauseMenuHandler>();
        pauseMenuScript.PauseGame();
    }
}
