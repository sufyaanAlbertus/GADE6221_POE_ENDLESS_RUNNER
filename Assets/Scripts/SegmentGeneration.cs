using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentGeneration : MonoBehaviour {
    public GameObject[] segment;
    private int zPos = 48;
    private bool creatingSegment = false;
    public Transform player; // Reference to the player's position

    void Update()
    {
        if (!creatingSegment)
        {
            StartCoroutine(SegmentGen());
        }
    }

    IEnumerator SegmentGen()
    {
        creatingSegment = true;

        int segmentNum = Random.Range(0, segment.Length);
        GameObject spawnedSegment = Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
        zPos += 48;

        // Start checking when to destroy this segment
        StartCoroutine(CheckForDestruction(spawnedSegment));

        yield return new WaitForSeconds(3);
        creatingSegment = false;
    }

    IEnumerator CheckForDestruction(GameObject segment)
    {
        while (segment != null)
        {
            if (segment.transform.position.z < player.position.z - 30) // Delete if it's behind the player
            {
                Destroy(segment);
                yield break; // Stop checking after destruction
            }
            yield return new WaitForSeconds(1); // Check every second
        }
    }

}
