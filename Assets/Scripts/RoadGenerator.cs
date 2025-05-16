using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [System.Serializable]
    public class RoadPrefab
    {
        public GameObject prefab;
        public int weight = 1; // ������� ���������
    }

    public GameObject firstTilePrefab;
    public GameObject finishTilePrefab;
    public List<RoadPrefab> roadPrefabs;
    public Transform player; // ������
    public float roadYOffset = -0.1f;
    public float tileLength = 8;
    public int tilesAhead = 10;
    public int maxTiles = 15;

    private Vector3 nextSpawnPosition;
    private readonly Queue<GameObject> spawnedTiles = new();
    private bool hasSpawnedFinish = false;
    public float finishDistance = 500f;


    public void Initialize(Transform playerTransform)
    {
        hasSpawnedFinish = false;
        player = playerTransform;
        nextSpawnPosition = player.position;
        spawnedTiles.Clear();

        SpawnSpecificTile(firstTilePrefab);

        for (int i = 0; i < tilesAhead; i++)
        {
            SpawnNextTile();
        }
    }


    void Update()
    {
        if (player == null || hasSpawnedFinish) return;
        float distanceAhead = nextSpawnPosition.z - player.position.z;
        if (nextSpawnPosition.z >= finishDistance && !hasSpawnedFinish)
        {
            SpawnFinishTile();
            hasSpawnedFinish = true;
            return; // Выходим — больше не спавним
        }
        if (distanceAhead < tilesAhead * tileLength)
        {
            SpawnNextTile();
            if (spawnedTiles.Count > maxTiles)
            {
                GameObject oldest = spawnedTiles.Dequeue();
                Destroy(oldest);
            }
        }
    }
    void SpawnFinishTile()
    {
        GameObject finish = Instantiate(
            finishTilePrefab,
            nextSpawnPosition + new Vector3(0, roadYOffset, 0),
            Quaternion.identity,
            this.transform
        );

        spawnedTiles.Enqueue(finish);
        nextSpawnPosition += new Vector3(0, 0, tileLength);
    }

    void SpawnNextTile()
    {
        GameObject prefabToSpawn = GetRandomPrefab();
        if (prefabToSpawn == null) return;

        GameObject spawned = Instantiate(prefabToSpawn, nextSpawnPosition + new Vector3(0, roadYOffset, 0), Quaternion.identity, this.transform);

        spawnedTiles.Enqueue(spawned);

        nextSpawnPosition += new Vector3(0, 0, tileLength);
    }
    void SpawnSpecificTile(GameObject prefab)
    {
        GameObject spawned = Instantiate(
            prefab,
            nextSpawnPosition + new Vector3(0, roadYOffset, 0),
            Quaternion.identity,
            this.transform
        );

        spawnedTiles.Enqueue(spawned);
        nextSpawnPosition += new Vector3(0, 0, tileLength);
    }


    GameObject GetRandomPrefab()
    {
        int totalWeight = roadPrefabs.Sum(r => r.weight);
        int randomWeight = Random.Range(0, totalWeight);

        foreach (var road in roadPrefabs)
        {
            if (randomWeight < road.weight)
            {
                return road.prefab;
            }
            randomWeight -= road.weight;
        }

        return null;
    }
}
