using UnityEngine;

public class GroundRepeater : MonoBehaviour
{
    public Transform player;
    public GameObject groundPrefab; // ������ �����������, ������� �� ���������� � ����������
    public float tileLength = 50f; // ����� ������ ����� �����
    public int groundCount = 3;

    private Transform[] groundTiles;
    private float recycleZ;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        if (groundPrefab == null)
        {
            Debug.LogError("Ground Prefab is not assigned in the Inspector!");
            return; // ��������� ����������, ���� ������ �� ��������
        }

        groundTiles = new Transform[groundCount];

        for (int i = 0; i < groundCount; i++)
        {
            // ���������� Instantiate � ��������, ����������� � ����������
            GameObject tileObject = Instantiate(groundPrefab, new Vector3(0, -0.4f, i * tileLength), Quaternion.identity, transform);
            groundTiles[i] = tileObject.transform; // ��������� Transform, � �� GameObject
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

