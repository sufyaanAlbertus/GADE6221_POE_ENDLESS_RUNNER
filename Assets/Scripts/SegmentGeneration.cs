using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Profiling.FrameDataView;

public class SegmentGeneration : MonoBehaviour
{
    public MasterInfo MasterInfo;
    public GameObject[] segmentPrefabs;
    public Transform player;
    public ObsticalSpawner obstacleSpawner;
    public PlayerMovement playerMovement;
   

    public int zPos = 0;
    private bool creatingSegment = false;

    public float segmentLength = 30f;  // Use 30 if your segment is scaled Z=30
    private float baseSpawnWait = 3f;
    private float minSpawnWait = 0.8f;
    private float baseDestroyOffset = 50f;
    private float minDestroyOffset = 25f;


    void Update()
    {
        if (!creatingSegment && player.position.z + baseDestroyOffset > segmentLength)
        {
            StartCoroutine(SegmentGen());
        }
    }

    IEnumerator SegmentGen()
    {
        creatingSegment = true;

        int index = Random.Range(0, segmentPrefabs.Length);
        Vector3 spawnPos = new Vector3(0, 0, zPos);
        GameObject spawnedSegment = Instantiate(segmentPrefabs[index], spawnPos, Quaternion.identity);


        // ? Only call obstacle spawning once with proper condition
        if (MasterInfo != null && !MasterInfo.stopObstacleSpawning && obstacleSpawner != null)
        {
            obstacleSpawner.SpawnObstaclesOnSegment(spawnPos);
        }

        zPos += Mathf.RoundToInt(segmentLength);  // Move forward by the actual segment length

        // Wait time scales with speed
        float speed = playerMovement.moveSpeed;
        float spawnWait = Mathf.Max(minSpawnWait, baseSpawnWait - (speed * 0.3f));

        // Start destruction check
        StartCoroutine(CheckForDestruction(spawnedSegment, speed));

        yield return new WaitForSeconds(spawnWait);
        creatingSegment = false;
    }

    IEnumerator CheckForDestruction(GameObject segment, float playerSpeed)
    {
        float destroyOffset = Mathf.Max(minDestroyOffset, baseDestroyOffset - (playerSpeed * 3f));

        while (segment != null)
        {
            if (segment.transform.position.z < player.position.z - destroyOffset)
            {
                Destroy(segment);
                yield break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}