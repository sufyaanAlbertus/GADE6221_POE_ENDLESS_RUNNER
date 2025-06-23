using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 5.73f, -27.06f);
    public float tiltOffsetX = 20f;

    public bool lockCameraOnHit = false;  // NEW ? lock camera when hit

    private Vector3 lockedPosition;
    private Vector3 lockedRotation;

    void LateUpdate()
    {
        if (GameManager.Instance.CurrentState == GameState.Playing || GameManager.Instance.CurrentState == GameState.Paused)
        {
            if (lockCameraOnHit)
            {
                // Stay fixed when hit
                transform.position = lockedPosition;
                transform.eulerAngles = lockedRotation;
            }
            else
            {
                // Normal follow
                Vector3 targetPos = player.position + offset;
                transform.position = targetPos;

                Vector3 rot = transform.eulerAngles;
                rot.x = tiltOffsetX;
                transform.eulerAngles = rot;
            }
        }
    }

    public void LockCameraPosition(Vector3 position, Vector3 rotation)
    {
        lockCameraOnHit = true;
        lockedPosition = position;
        lockedRotation = rotation;
    }

    public void UnlockCamera()
    {
        lockCameraOnHit = false;
    }
}
