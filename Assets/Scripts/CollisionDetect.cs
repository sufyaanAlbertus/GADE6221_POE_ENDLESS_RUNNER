using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CollisionDetect : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject playerAnim;
    [SerializeField] GameObject MainCam;
    [SerializeField] GameObject FadeOut;

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(CollisionEnd());
    }


    IEnumerator CollisionEnd()
    {

        player.GetComponent<PlayerMovement>().enabled = false;
        playerAnim.GetComponent<Animator>().Play("Stumble Backwards");
        MainCam.GetComponent<Animator>().Play("CollisionCam");
        yield return new WaitForSeconds(3);
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

}
