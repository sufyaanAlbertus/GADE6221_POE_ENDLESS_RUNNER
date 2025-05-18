using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObsticalSpawner : MonoBehaviour
{

    public MasterInfo gameManager;
    public GameObject[] obstaclePrefabs;
    public float[] lanePositions = { -4f, 0f, 4f };
    public float segmentLength = 30f;
    public int obstaclesPerSegment = 3;
    public GameObject player;  // Assign this in inspector or via code

    

    public List<GameObject> SpawnObstaclesOnSegment(Vector3 segmentStartPos)
    {
        List<GameObject> spawnedObstacles = new List<GameObject>();

        if (gameManager.stopObstacleSpawning)
            return spawnedObstacles;

        int lanesAvailable = lanePositions.Length;
        int obstaclesToSpawn = Mathf.Min(obstaclesPerSegment, lanesAvailable);

        List<int> availableLaneIndices = Enumerable.Range(0, lanesAvailable).ToList();
        ShuffleList(availableLaneIndices);

        float spacing = segmentLength / obstaclesToSpawn;
        float startZ = segmentStartPos.z;

        for (int i = 0; i < obstaclesToSpawn; i++)
        {
            int laneIndex = availableLaneIndices[i];
            float x = lanePositions[laneIndex];
            float z = startZ + i * spacing;

            int prefabIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject prefab = obstaclePrefabs[prefabIndex];

            float y = GetYOffsetForPrefab(prefabIndex);

            Vector3 spawnPos = new Vector3(x, y, z);

            GameObject obstacle = Instantiate(prefab, spawnPos, Quaternion.Euler(0, 90, 0));
            obstacle.AddComponent<CloneMarker>();
            spawnedObstacles.Add(obstacle);

           
        }

        return spawnedObstacles;
    }


    private float GetYOffsetForPrefab(int index)
    {
        // Adjust based on your prefab size
        switch (index)
        {
            case 0: return 0.49f; // drawer
            case 1: return 0.45f; // desk
            case 2: return 0.493f; // printer
            case 3: return 0.485f; // sofa
            case 4: return 1.5f; // coin
            default: return 0f;
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}

