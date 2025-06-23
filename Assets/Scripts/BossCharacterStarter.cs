using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacterStarter : MonoBehaviour
{
    public AudioSource normalMusicSource;   // normal BG music
    public AudioSource bossMusicSource;     // boss fight music
    public MasterInfo masterInfo;
    public SegmentGeneration segmentGenerations;  // Add this
    public GameObject bossPrefab;
    public GameObject[] bossObstaclePrefabs;
    public PlayerMovement playerMovement;

    public Transform player;
    public Camera mainCamera;


    public float[] lanePositions = { -4f, 0f, 4f };

    public float zOffsetFromPlayer = 15f;
    public float laneMoveInterval = 10f;
    public float obstacleSpawnInterval = 0.5f;
    public float laneSwitchSpeed = 15f;

    private float bossCameraY = 4.23f;
    private float bossCameraZ = -3.31f;
    private float bossRotationYDegrees = 180f;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private bool isPausedForHit = false;

    private bool bossMusicStarted = false; 
    private bool normalMusicStarted = false;
    private GameObject bossInstance;
    public bool bossSpawned = false;
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
        if (isPausedForHit) return;

        if (masterInfo != null)
        {
            // Play boss music early at score 48
            if (!bossMusicStarted && masterInfo.CurrentScore >= 48)
            {
                bossMusicStarted = true;
                StartBossMusic();
            }

            // Spawn boss at 50
            if (!bossSpawned && masterInfo.stopObstacleSpawning)
            {
                SpawnBoss();
            }
        }


        if (bossSpawned && !masterInfo.bossDefeated && masterInfo.CurrentScore >= 100 )
        {

           
            DefeatBoss();
        }

        if (bossInstance != null && player != null)
        {
            FollowAndMoveBoss();
        }
    }



    void StartBossMusic()
    {
        if (normalMusicSource != null)
        {
            normalMusicSource.Stop();
        }

        if (bossMusicSource != null)
        {
            bossMusicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            bossMusicSource.Play();

            Debug.Log("Boss music started!");
        }

        bossMusicStarted = true; // Set flag
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

    // Call this from CollisionDetect when hit occurs
    public void PauseBossActions(float pauseDuration)
    {
        if (!bossSpawned) return;

        StartCoroutine(PauseBossSequence(pauseDuration));
    }

    private IEnumerator PauseBossSequence(float pauseDuration)
    {
        isPausedForHit = true;

        // 1. Stop obstacle spawning
        if (bossObstacleCoroutine != null)
        {
            StopCoroutine(bossObstacleCoroutine);
            bossObstacleCoroutine = null;
        }

        // 2. Freeze boss movement
        Vector3 frozenPosition = bossInstance.transform.position;

        float timer = 0f;
        while (timer < pauseDuration)
        {
            bossInstance.transform.position = frozenPosition; // Enforce no movement
            timer += Time.unscaledDeltaTime; // Works even if game is paused
            yield return null;
        }

        // 3. Resume boss behavior
        isPausedForHit = false;

        if (bossInstance != null)
        {
            bossObstacleCoroutine = StartCoroutine(SpawnBossObstacles());
        }
    }

    public void DiePlayerBossSequence()
    {
        if (bossInstance == null)
        {
            Debug.LogWarning("DiePlayerBossSequence called but bossInstance is NULL — skipping.");
            return;
        }

        // Stop obstacle spawning
        if (bossObstacleCoroutine != null)
        {
            StopCoroutine(bossObstacleCoroutine);
            bossObstacleCoroutine = null;
        }

        // Freeze boss movement
        Vector3 frozenPosition = bossInstance.transform.position;
        bossInstance.transform.position = frozenPosition; // Enforce no movement
    }

    void FollowAndMoveBoss()
    {

        if (isPausedForHit) return;

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

        // STOP boss music and resume normal music
        if (bossMusicSource != null)
        {
            bossMusicSource.Stop();
        }

        if (normalMusicSource != null)
        {
            normalMusicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            normalMusicSource.Play();

            Debug.Log("Normal music resumed after boss!");
        }

        bossMusicStarted = false; // Reset flag


        // Reset the segment spawning position to the player's current z position
        if (segmentGenerations != null && player != null)
        {
            segmentGenerations.zPos = Mathf.FloorToInt(player.position.z);
        }

        Debug.Log("Boss defeated! Camera reset. Player speed increased. Obstacles and segments resumed.");
    }

}

