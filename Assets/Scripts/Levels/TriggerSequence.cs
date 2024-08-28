using Cameras;
using UnityEngine;
using System.Collections;

public class TriggerSequence : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerFollowCamera playerCamera;

    public Transform enemy;
    public Transform wallPosition;
    public Transform playerLookPosition;
    public float cameraMoveSpeed = 2f;
    public float bangDelay = 0.5f;
    public Animator enemyAnimator;

    private bool sequenceStarted = false;

    private void Start()
    {
        // Attempt to find Player and Camera clones after they have spawned in the scene
        Invoke("AssignComponents", 0.1f); // Small delay to ensure clones are instantiated
    }

    private void AssignComponents()
    {
        // Find the player clone
        GameObject playerObject = GameObject.Find("Player(Clone)");
        if (playerObject != null)
        {
            playerMovement = playerObject.GetComponent<PlayerMovement>();
            Debug.Log("PlayerMovement component found: " + (playerMovement != null));
        }
        else
        {
            Debug.LogError("Player object not found! Ensure the Player prefab is correctly instantiated.");
        }

        // Find the camera clone
        GameObject cameraObject = GameObject.Find("PlayerFollowCamera(Clone)");
        if (cameraObject != null)
        {
            playerCamera = cameraObject.GetComponent<PlayerFollowCamera>();
            Debug.Log("PlayerFollowCamera component found: " + (playerCamera != null));
        }
        else
        {
            Debug.LogError("Camera object not found! Ensure the Camera prefab is correctly instantiated.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !sequenceStarted)
        {
            sequenceStarted = true;
            StartCoroutine(SequenceCoroutine(other.transform));
        }
    }

    private IEnumerator SequenceCoroutine(Transform player)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        player.position = playerLookPosition.position;

        if (playerCamera != null)
        {
            playerCamera.UpdateYOffset(0f);
            while (Vector2.Distance(playerCamera.transform.position, wallPosition.position) > 0.1f)
            {
                playerCamera.transform.position = Vector3.MoveTowards(playerCamera.transform.position, wallPosition.position, cameraMoveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Bang");
            }
            yield return new WaitForSeconds(bangDelay);
        }

        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Burst");
        }

        yield return new WaitForSeconds(1f);
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Walk");
        }

        while (Vector2.Distance(enemy.position, playerLookPosition.position) > 1f)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, playerLookPosition.position, 1f * Time.deltaTime);
            yield return null;
        }

        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Look");
        }

        yield return new WaitForSeconds(1f);
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Chase");
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (playerCamera != null)
        {
            playerCamera.UpdateYOffset(2f);
            playerCamera.CineVirtualCamera.Follow = player;
        }
    }
}
