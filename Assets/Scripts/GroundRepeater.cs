using UnityEngine;

public class GroundRepeater : MonoBehaviour
{
    public Transform player;
    public GameObject groundPrefab; // Префаб поверхности, который вы назначаете в инспекторе
    public float tileLength = 50f; // длина одного куска земли
    public int groundCount = 3;

    private Transform[] groundTiles;
    private float recycleZ;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        if (groundPrefab == null)
        {
            Debug.LogError("Ground Prefab is not assigned in the Inspector!");
            return; // Прерываем выполнение, если префаб не назначен
        }

        groundTiles = new Transform[groundCount];

        for (int i = 0; i < groundCount; i++)
        {
            // Используем Instantiate с префабом, назначенным в инспекторе
            GameObject tileObject = Instantiate(groundPrefab, new Vector3(0, -0.4f, i * tileLength), Quaternion.identity, transform);
            groundTiles[i] = tileObject.transform; // Сохраняем Transform, а не GameObject
        }

        recycleZ = tileLength * groundCount;
    }

    void Update()
    {
        if (player == null) return;

        foreach (Transform tile in groundTiles)
        {
            if (player.position.z - tile.position.z > tileLength)
            {
                tile.position += new Vector3(0, 0, recycleZ);
            }
        }
    }
}

