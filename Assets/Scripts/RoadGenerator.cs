using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoadGenerator : MonoBehaviour
{
    [System.Serializable]
    public class RoadPrefab
    {
        public GameObject prefab;
        public int weight = 1; // Частота появления
    }

    public GameObject firstTilePrefab;
    public List<RoadPrefab> roadPrefabs;
    public Transform player; // Машина
    public float roadYOffset = -0.1f;
    public float tileLength = 8;
    public int tilesAhead = 10;
    public int maxTiles = 15;

    private Vector3 nextSpawnPosition;
    private readonly Queue<GameObject> spawnedTiles = new Queue<GameObject>();

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player не назначен!");
            return;
        }

        nextSpawnPosition = player.position;

        SpawnSpecificTile(firstTilePrefab);

        for (int i = 0; i < tilesAhead; i++)
        {
            SpawnNextTile();
        }
    }
    void Update()
    {
        float distanceAhead = nextSpawnPosition.z - player.position.z;

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
