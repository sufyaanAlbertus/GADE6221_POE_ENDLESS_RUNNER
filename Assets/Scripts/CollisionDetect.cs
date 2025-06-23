using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject score;
    [SerializeField] GameObject playerAnim;
    [SerializeField] GameObject MainCam;
    [SerializeField] GameObject FadeOut;




    private PlayerHealth playerHealth;
    private bool deathSequenceRunning = false;

    private void Start()
    {
        // Get PlayerHealth from player in scene
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (deathSequenceRunning)
            return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with obstacle!");

            // Lose 1 life
            playerHealth.LoseLife(1);

            if (playerHealth.currentLives == 0)
            {
                StartDeathSequence(other.gameObject);
            }
            else
            {
                StartCoroutine(HitReaction(other.gameObject));
            }
        }
    }

    private void StartDeathSequence(GameObject player)
    {
        if (deathSequenceRunning)
            return;


        if (player == null)
        {
            Debug.LogError("StartDeathSequence called with null player!");
            return;
        }

        deathSequenceRunning = true;
        StartCoroutine(CollisionEnd(player));
    }

    private IEnumerator HitReaction(GameObject player)
    {
            Debug.Log("Player hit! Life lost.");

            GameManager.Instance.PauseGame();

        BossCharacterStarter bossStarter = FindObjectOfType<BossCharacterStarter>();

        if (bossStarter != null)
        {
            bossStarter.PauseBossActions(1.7f);
        }

        BossCharacter2 BossCharacter2 = FindObjectOfType<BossCharacter2>();
        if (BossCharacter2 != null)
        {
            BossCharacter2.PauseBossActions(1.7f);
        }

        yield return new WaitForSeconds(1.5f);
           

            GameManager.Instance.ResumeGame();
           
            Debug.Log("Player recovered. Remaining lives: " + playerHealth.currentLives);
     }


    private IEnumerator CollisionEnd(GameObject player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        score.GetComponent<MasterInfo>().HandleCollisionImpact();

        playerAnim.GetComponent<Animator>().Play("Stumble Backwards");
        MainCam.GetComponent<Animator>().Play("CollisionCam");

        BossCharacterStarter bossStarter = FindObjectOfType<BossCharacterStarter>();


        BossCharacter2 BossCharacter2 = FindObjectOfType<BossCharacter2>();
        if (BossCharacter2 != null && BossCharacter2.gameObject.activeInHierarchy)
        {
            BossCharacter2.DiePlayerBossSequence();
        }
        else
        {
            Debug.LogWarning("BossCharacterStarter2 not found or inactive — skipping DiePlayerBossSequence.");
        }

        // Only call if bossInstance exists!
        if (bossStarter != null && bossStarter.gameObject.activeInHierarchy)
        {
            bossStarter.DiePlayerBossSequence();
        }
        else
        {
            Debug.LogWarning("BossCharacterStarter not found or inactive — skipping DiePlayerBossSequence.");
        }

        // ? Save score and coins before fading out
        if (ScoreManager.Instance != null)
        {
            Debug.Log("Saving score and coins...");
            ScoreManager.Instance.SaveScoreAndCoins();
        }
        else
        {
            Debug.LogError("ScoreManager.Instance is null — cannot save score!");
        }


        yield return new WaitForSeconds(3);
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameMode");
        MasterInfo.Instance.ResetAll();

        // Always re-set userId (in case ScoreManager was reloaded)
        ScoreManager.Instance.userId = GlobalUser.userId;

        // Fetch latest coins
        if (ScoreManager.Instance.userId != 0)
        {
            ScoreManager.Instance.FetchTotalCoinsFromDB();
        }
    }
}
