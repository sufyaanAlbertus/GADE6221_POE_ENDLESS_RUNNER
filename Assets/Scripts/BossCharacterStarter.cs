using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacterStarter : MonoBehaviour
{
    
       public MasterInfo masterInfo;
    public SegmentGeneration segmentGeneration;  // Add this
    public GameObject bossPrefab;
    public GameObject[] bossObstaclePrefabs;
    public PlayerMovement playerMovement;

    public Transform player;
    public Camera mainCamera;


    public float[] lanePositions = { -4f, 0f, 4f };

    public float zOffsetFromPlayer = 15f;
    public float laneMoveInterval = 2f;
    public float obstacleSpawnInterval = 1.5f;
    public float laneSwitchSpeed = 3f;

    private float bossCameraY = 4.23f;
    private float bossCameraZ = -3.31f;
    private float bossRotationYDegrees = 180f;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;


    private GameObject bossInstance;
    private bool bossSpawned = false;
    private Coroutine bossObstacleCoroutine;
    private Vector3 targetPosition;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.position;
            originalCameraRotation = mainCamera.transform.rotation;
        }
    }

    void Update()
    {
        if (!bossSpawned && masterInfo != null && masterInfo.stopObstacleSpawning)
        {
            SpawnBoss();
        }

        if (bossSpawned && !masterInfo.bossDefeated && masterInfo.CurrentScore >= 100)
        {
            DefeatBoss();
        }

        if (bossInstance != null && player != null)
        {
            FollowAndMoveBoss();
        }
    }


    void SpawnBoss()
    {
        bossSpawned = true;

        if (playerMovement != null)
            playerMovement.canIncreaseSpeed = false;

        // Move camera for boss fight
        if (mainCamera != null)
        {
            Vector3 camPos = mainCamera.transform.position;
            mainCamera.transform.position = new Vector3(camPos.x, bossCameraY, bossCameraZ);
        }

        // Spawn boss
        Vector3 spawnPos = new Vector3(0f, 0.5f, player.position.z + zOffsetFromPlayer);
        bossInstance = Instantiate(bossPrefab, spawnPos, Quaternion.Euler(0, bossRotationYDegrees, 0));

        bossObstacleCoroutine = StartCoroutine(SpawnBossObstacles());
    }

    void FollowAndMoveBoss()
    {
        float targetZ = player.position.z + zOffsetFromPlayer;
        float laneX = Mathf.PingPong(Time.time * laneSwitchSpeed, 11f) - 5.5f;

        targetPosition = new Vector3(laneX, bossInstance.transform.position.y, targetZ);
        bossInstance.transform.position = Vector3.Lerp(bossInstance.transform.position, targetPosition, Time.deltaTime * 5f);
    }


    IEnumerator SpawnBossObstacles()
    {
        while (bossInstance != null)
        {
            if (bossObstaclePrefabs.Length > 0)
            {
                int prefabIndex = Random.Range(0, bossObstaclePrefabs.Length);
                GameObject prefab = bossObstaclePrefabs[prefabIndex];

                float yOffset = GetYOffsetForPrefab(prefabIndex);

                Vector3 spawnPos = new Vector3(
                    bossInstance.transform.position.x,
                    yOffset,
                    bossInstance.transform.position.z - 5f
                );

                // Instantiate with a 90-degree rotation on the Y-axis
                Quaternion rotation = Quaternion.Euler(0, 90, 0);
                Instantiate(prefab, spawnPos, rotation);
            }

            yield return new WaitForSeconds(obstacleSpawnInterval);
        }
    }

    private float GetYOffsetForPrefab(int index)
    {
        switch (index)
        {
            case 0: return 0.49f; // drawer
            case 1: return 0.45f; // desk
            case 2: return 0.493f; // printer
            case 3: return 0.485f; // sofa
            default: return 0f;
        }
    }


    void DefeatBoss()
    {
        masterInfo.bossDefeated = true;

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);

        if (bossInstance != null)
        {
            Destroy(bossInstance);
            bossInstance = null;
        }

        if (bossObstacleCoroutine != null)
        {
            StopCoroutine(bossObstacleCoroutine);
            bossObstacleCoroutine = null;
        }

        if (playerMovement != null)
            playerMovement.canIncreaseSpeed = true;

        masterInfo.stopObstacleSpawning = false;
        masterInfo.clearedObstacles = false;

        // Reset the segment spawning position to the player's current z position
        if (segmentGeneration != null && player != null)
        {
            segmentGeneration.zPos = Mathf.FloorToInt(player.position.z);
        }

        Debug.Log("Boss defeated! Camera reset. Player speed increased. Obstacles and segments resumed.");
    }

}

