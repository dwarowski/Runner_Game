using UnityEngine;

public class GroundRepeater : MonoBehaviour
{
    public Transform player;
    public float tileLength = 50f; // длина одного куска земли
    public int groundCount = 3;

    private Transform[] groundTiles;
    private float recycleZ;

    void Start()
    {
        groundTiles = new Transform[groundCount];

        for (int i = 0; i < groundCount; i++)
        {
            Transform tile = Instantiate(transform.GetChild(0), new Vector3(0, -0.4f, i * tileLength), Quaternion.identity, transform);
            groundTiles[i] = tile;
        }

        recycleZ = tileLength * groundCount;
    }

    void Update()
    {
        foreach (Transform tile in groundTiles)
        {
            if (player.position.z - tile.position.z > tileLength)
            {
                tile.position += new Vector3(0, 0, recycleZ);
            }
        }
    }
}
