using UnityEngine;

public class CarEvolutionHandler : MonoBehaviour
{
    public CarEvolutionData evolutionData;
    public int currentStage = 0;

    public UI ui; // UI панель со спидометром и т.д.
    public RoadGenerator roadGenerator;
    public GroundRepeater surfaceRepeater;
    private GameObject currentCarInstance;
    public void EvolveCar(GameObject vfxPrefab, Vector3 vfxPosition, Transform vfxParent)
    {
        if (transform.childCount > 0)
        {
            currentCarInstance = transform.GetChild(0).gameObject;
        }
        else
        {
            currentCarInstance = null;
        }
        if (evolutionData == null || evolutionData.evolutionPrefabs.Length == 0) return;
        if (currentStage >= evolutionData.evolutionPrefabs.Length) return;

        GameObject nextPrefab = evolutionData.evolutionPrefabs[currentStage];
        currentStage++;

        // Визуальный эффект
        if (vfxPrefab != null)
        {
            GameObject vfx = Instantiate(vfxPrefab, vfxPosition, Quaternion.identity, vfxParent);
            if (vfx.TryGetComponent<ParticleSystem>(out var ps))
            {
                Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(vfx, 3f); // запасной вариант
            }
        }

        // Создаём новую машину 
        Vector3 spawnPos = currentCarInstance.transform.position;
        Quaternion spawnRot = currentCarInstance.transform.rotation;
        Vector3 currentVelocity = Vector3.zero;

        // Сохраняем движение старой машины
        if (currentCarInstance.TryGetComponent<Rigidbody>(out var oldRb))
            currentVelocity = oldRb.linearVelocity;

        // Создаем новую машину
        GameObject newCar = Instantiate(nextPrefab, spawnPos, spawnRot, transform);

        // Назначаем UI в новый CarControl
        if (newCar.TryGetComponent<CarControl>(out var newCarControl))
        {
            newCarControl.ui = ui;
            newCarControl.Initialize();
        }

        // Переназначем генераторы на новую машину
        Transform carTransform = newCar.GetComponent<Transform>();
        roadGenerator.player = carTransform;
        surfaceRepeater.player = carTransform;

        // Восстановить дистанцию
        float savedDistance = 0f;
        if (currentCarInstance != null && currentCarInstance.TryGetComponent<CarControl>(out var oldCarControl))
        {
            savedDistance = oldCarControl.GetTotalDistance();
        }
        newCarControl.SetTotalDistance(savedDistance);

        // Возвращем движение машине
        if (newCar.TryGetComponent<Rigidbody>(out var newRb))
            newRb.linearVelocity = currentVelocity;

        // Перемещаем MainCamera к новой машине (если есть CameraAnchor)
        Transform cameraAnchor = newCar.transform.Find("CameraAnchor");
        if (cameraAnchor != null)
        {
            Camera.main.transform.SetParent(cameraAnchor);
            Camera.main.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        // Удаляем старую машину, если есть
        if (currentCarInstance != null)
        {
            Destroy(currentCarInstance);
        }
        currentCarInstance = newCar;

        Debug.Log($"Эволюция: новая машина создана на стадии {currentStage}");
    }
}
