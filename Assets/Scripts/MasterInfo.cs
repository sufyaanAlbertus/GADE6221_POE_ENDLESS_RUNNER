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



    void Update()
    {
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

    //public void ClearAllsegements()
    //{


    //    SegmentCloneMaker[] clones = FindObjectsOfType<SegmentCloneMaker>();
    //    foreach (SegmentCloneMaker marker in clones)
    //    {
    //        Destroy(marker.gameObject);
    //    }



    //    Debug.Log("All spawned obstacle clones have been destroyed.");
    //}



}
