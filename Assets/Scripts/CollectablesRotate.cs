using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesRotate : MonoBehaviour
{
    [SerializeField] int rotateSpeed = 5;
    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }

}
