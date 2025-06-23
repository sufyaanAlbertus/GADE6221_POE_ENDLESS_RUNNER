using SojaExiles;
using System.Collections;
using UnityEngine;
public class BossCharacter2 : MonoBehaviour
{
    public AudioSource normalMusicSource2;
    public AudioSource bossMusicSource2;
    public MasterInfo masterInfo2;
    public SegmentGeneration segmentGeneration2;
    public GameObject bossPrefab2;
    public GameObject[] bossObstaclePrefabs2;
    public PlayerMovement playerMovement2;

    public Transform player2;
    public Camera mainCamera2;

    public float[] lanePositions2 = { -4f, 0f, 4f };

    public float zOffsetFromPlayer2 = 15f;
    public float laneSwitchInterval2 = 2.5f;
    public float laneSwitchSpeed2 = 15f;

    private float bossCameraY2 = 4.23f;
    private float bossCameraZ2 = -3.31f;
    private float bossRotationYDegrees2 = 180f;

    private Vector3 originalCameraPosition2;
    private Quaternion originalCameraRotation2;

    private bool isPausedForHit2 = false;
    private bool bossMusicStarted2 = false;
    private bool normalMusicStarted2 = false;

    private GameObject bossInstance2;
    public bool bossSpawned2 = false;
    private Coroutine laneSwitchCoroutine2;
    private Vector3 targetPosition2;
    private int currentLaneIndex2 = 1;

    void Start()
    {
        if (mainCamera2 == null)
            mainCamera2 = Camera.main;

        if (mainCamera2 != null)
        {
            originalCameraPosition2 = mainCamera2.transform.position;
            originalCameraRotation2 = mainCamera2.transform.rotation;
        }
    }

    void Update()
    {
        if (isPausedForHit2) return;

        if (masterInfo2 != null)
        {
            // Play boss 2 music early at score 148
            if (!bossMusicStarted2 && masterInfo2.CurrentScore >= 148)
            {
                bossMusicStarted2 = true;
               
                StartBossMusic();
               
            }

            // Spawn boss at 150
            if (!bossSpawned2 && masterInfo2.stopObstacleSpawning2)
            {
                SpawnBoss();
            }
        }

        if (bossSpawned2 && !masterInfo2.bossDefeated2 && masterInfo2.CurrentScore >= 200)
        {
           
           
            DefeatBoss();
            
        }

        if (bossInstance2 != null && player2 != null)
        {
            FollowAndMoveBoss();
        }
    }


    void StartBossMusic()
    {
        if (normalMusicSource2 != null)
        {
            normalMusicSource2.Stop();
        }

        if (bossMusicSource2 != null)
        {
            bossMusicSource2.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            bossMusicSource2.Play();

            Debug.Log("Boss music started!");
        }

        bossMusicStarted2 = true; // Set flag


    }

    void SpawnBoss()
    {
        bossSpawned2 = true;

        if (playerMovement2 != null)
            playerMovement2.canIncreaseSpeed = false;

        if (mainCamera2 != null)
        {
            Vector3 camPos = mainCamera2.transform.position;
            mainCamera2.transform.position = new Vector3(camPos.x, bossCameraY2, bossCameraZ2);
        }

        Vector3 spawnPos = new Vector3(lanePositions2[currentLaneIndex2], 0.5f, player2.position.z + zOffsetFromPlayer2);
        bossInstance2 = Instantiate(bossPrefab2, spawnPos, Quaternion.Euler(0, bossRotationYDegrees2, 0));

        laneSwitchCoroutine2 = StartCoroutine(LaneSwitchRoutine());
    }

    public void PauseBossActions(float pauseDuration)
    {
        if (!bossSpawned2) return;

        StartCoroutine(PauseBossSequence(pauseDuration));
    }

    private IEnumerator PauseBossSequence(float pauseDuration)
    {
        isPausedForHit2 = true;

        if (laneSwitchCoroutine2 != null)
        {
            StopCoroutine(laneSwitchCoroutine2);
            laneSwitchCoroutine2 = null;
        }

        Vector3 frozenPosition = bossInstance2.transform.position;

        float timer = 0f;
        while (timer < pauseDuration)
        {
            bossInstance2.transform.position = frozenPosition;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        isPausedForHit2 = false;

        if (bossInstance2 != null)
        {
            laneSwitchCoroutine2 = StartCoroutine(LaneSwitchRoutine());
        }
    }

    public void DiePlayerBossSequence()
    {
        if (bossInstance2 == null)
        {
            Debug.LogWarning("DiePlayerBossSequence called but bossInstance2 is NULL.");
            return;
        }

        if (laneSwitchCoroutine2 != null)
        {
            StopCoroutine(laneSwitchCoroutine2);
            laneSwitchCoroutine2 = null;
        }

        Vector3 frozenPosition = bossInstance2.transform.position;
        bossInstance2.transform.position = frozenPosition;
    }

    void FollowAndMoveBoss()
    {
        if (isPausedForHit2) return;

        float targetZ = player2.position.z + zOffsetFromPlayer2;
        Vector3 moveTarget = new Vector3(lanePositions2[currentLaneIndex2], bossInstance2.transform.position.y, targetZ);

        bossInstance2.transform.position = Vector3.Lerp(bossInstance2.transform.position, moveTarget, Time.deltaTime * laneSwitchSpeed2);
    }

    IEnumerator LaneSwitchRoutine()
    {
        while (bossInstance2 != null)
        {
            currentLaneIndex2 = Random.Range(0, lanePositions2.Length);
            targetPosition2 = new Vector3(lanePositions2[currentLaneIndex2], bossInstance2.transform.position.y, player2.position.z + zOffsetFromPlayer2);

            SpawnObstaclesInCurrentLane();

            yield return new WaitForSeconds(laneSwitchInterval2);
        }
    }

    void SpawnObstaclesInCurrentLane()
    {
        if (bossObstaclePrefabs2.Length == 0)
            return;

        int obstaclesToSpawn = Random.Range(2, 6);
        float spacing = 2.5f;

        for (int i = 0; i < obstaclesToSpawn; i++)
        {
            int prefabIndex = Random.Range(0, bossObstaclePrefabs2.Length);
            GameObject prefab = bossObstaclePrefabs2[prefabIndex];

            float yOffset = GetYOffsetForPrefab(prefabIndex);

            Vector3 spawnPos = new Vector3(
                lanePositions2[currentLaneIndex2],
                yOffset,
                player2.position.z + zOffsetFromPlayer2 + (i * spacing)
            );

            Quaternion rotation = Quaternion.Euler(0, 90, 0);
            Instantiate(prefab, spawnPos, rotation);
        }
    }

    private float GetYOffsetForPrefab(int index)
    {
        switch (index)
        {
            case 0: return 0.49f;
            case 1: return 0.45f;
            case 2: return 0.493f;
            case 3: return 0.485f;
            case 4: return 1.5f;
            case 5: return 1.5f;
            default: return 0f;
        }
    }

    void DefeatBoss()
    {
        masterInfo2.bossDefeated = true;

        if (mainCamera2 != null)
            mainCamera2.gameObject.SetActive(true);

        if (bossInstance2 != null)
        {
            Destroy(bossInstance2);
            bossInstance2 = null;
        }

        if (laneSwitchCoroutine2 != null)
        {
            StopCoroutine(laneSwitchCoroutine2);
            laneSwitchCoroutine2 = null;
        }

        if (playerMovement2 != null)
            playerMovement2.canIncreaseSpeed = true;

        masterInfo2.stopObstacleSpawning = false;
        masterInfo2.clearedObstacles = false;

        // STOP boss music and resume normal music
        if (bossMusicSource2 != null)
        {
            bossMusicSource2.Stop();
        }

        if (normalMusicSource2 != null)
        {
            normalMusicSource2.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            normalMusicSource2.Play();

            Debug.Log("Normal music resumed after boss!");
        }

        bossMusicStarted2 = false; // Reset flag

        if (segmentGeneration2 != null && player2 != null)
        {
            segmentGeneration2.zPos = Mathf.FloorToInt(player2.position.z);
        }

        Debug.Log("Boss 2 defeated! Camera reset, speed restored, obstacles resumed.");
    }
}

