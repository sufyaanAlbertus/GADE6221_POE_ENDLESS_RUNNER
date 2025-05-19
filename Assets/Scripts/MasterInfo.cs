using SojaExiles;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterInfo : MonoBehaviour
{

    public static int coinCount = 0;
  
    [SerializeField] GameObject coinDisplay;
    [SerializeField] GameObject scoreDisplay;
    private float gameTime = 0f;
    public bool stopObstacleSpawning = false;
    public bool clearedObstacles = false;
    public int CurrentScore { get; private set; }
    public bool bossTriggered = false;
    public bool bossDefeated = false;
    public bool stopScore = false;

    public static MasterInfo Instance { get; private set; }

    private void Awake()
    {
       
        Instance = this;
        
    }


    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (coinDisplay != null)
            coinDisplay.GetComponent<TMP_Text>().text = "COINS: " + coinCount;

        if (!stopScore)  
            gameTime += Time.deltaTime;
        
        CurrentScore = Mathf.FloorToInt(gameTime);

        if (scoreDisplay != null)
            scoreDisplay.GetComponent<TMP_Text>().text = "Score: " + CurrentScore;


        // Stop obstacle spawning when score hits 50
        if (CurrentScore == 50 && !stopObstacleSpawning)
        {
            stopObstacleSpawning = true;
            Debug.Log("Score hit 50! Obstacles will stop spawning.");
            ClearAllObstacles();
        }

       
    }

    public void ClearAllObstacles()
    {
        if (clearedObstacles) return;

        CloneMarker[] clones = FindObjectsOfType<CloneMarker>();
        foreach (CloneMarker marker in clones)
        {
            Destroy(marker.gameObject);
        }


        clearedObstacles = true;
        Debug.Log("All spawned obstacle clones have been destroyed.");
    }
    public void HandleCollisionImpact()
    {
        stopScore = true;
        Debug.Log("Collision detected! Score updates and obstacle spawning stopped.");
    }

    public void ResetAll()
    {
        coinCount = 0;
        gameTime = 0f;
        CurrentScore = 0;

        stopObstacleSpawning = false;
        clearedObstacles = false;
        bossTriggered = false;
        bossDefeated = false;
        stopScore = false;

        // Reset UI
        if (coinDisplay != null)
            coinDisplay.GetComponent<TMP_Text>().text = "COINS: 0";

        if (scoreDisplay != null)
            scoreDisplay.GetComponent<TMP_Text>().text = "Score: 0";

        // Clean up all obstacles and segments
        ClearAllObstacles(); // Uses CloneMarker tag/component
        ClearAllSegments();  // Uses CloneMarker or any identifier on segments

        Debug.Log("Game state has been fully reset.");
    }

    

    public void ClearAllSegments()
    {
        SegmentCloneMaker[] segmentClones = FindObjectsOfType<SegmentCloneMaker>();
        foreach (SegmentCloneMaker marker in segmentClones)
        {
            Destroy(marker.gameObject);
        }

        Debug.Log("All segment clones destroyed.");
    }

}
