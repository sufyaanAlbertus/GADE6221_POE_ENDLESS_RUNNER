using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    private float gameStartTime;


    // Initial boundary for first 4 seconds
    private float initialLeftLimit = -2.5f;
    private float initialRightLimit = 1.72f;

    // Full boundary after 4 seconds
    private float leftLimit = -5.5f;
    private float rightLimit = 5.5f;

    void Start()
    {
       
        gameStartTime = Time.time; // Store the start time
    }
    // Update is called once per frame
    void Update()
    {
       
        MovePlayer();
       
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        Vector3 movement = new Vector3(horizontal, 0, 1) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        ////boundaries
        //float clampedX = Mathf.Clamp(transform.position.x, leftLimit, rightLimit);
        //transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // Determine which boundary to use
        float elapsedTime = Time.time - gameStartTime;
        float leftBoundary = elapsedTime < 1.6f ? initialLeftLimit : leftLimit;
        float rightBoundary = elapsedTime < 1.6f ? initialRightLimit : rightLimit;

        // Restrict movement within the selected boundary
        float clampedX = Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);


    }
}

   

