using UnityEngine;
using System.Collections;
using Cinemachine; // For Cinemachine Virtual Camera control
using UnityEngine.Rendering.Universal; // For Light2D
using Characters.Player; // For accessing Player singleton
using Cameras; // For accessing PlayerFollowCamera singleton
using Interaction; // For IInteractable

public class PostItEvent : MonoBehaviour, IInteractable
{
    public Light2D globalLight; // Reference to the Global Light 2D component
    public float darkeningSpeed = 1f; // Speed at which the light will dim
    public GameObject enemy; // The enemy that will be revealed
    public Transform enemyRevealPosition; // Position the camera should move to during the cutscene
    public float cutsceneDuration = 3f; // Duration of the cutscene before the player regains control

    private bool isPostItRemoved = false;
    private PlayerInputController inputController; // Reference to the player's input controller
    private CinemachineVirtualCamera cineVirtualCamera; // Reference to the Cinemachine virtual camera

    private void Start()
    {
        // Get the player and its components
        inputController = Player.Instance.GetComponent<PlayerInputController>(); // Access PlayerInputController

        // Get the Cinemachine Virtual Camera from the PlayerFollowCamera singleton
        cineVirtualCamera = PlayerFollowCamera.Instance.CineVirtualCamera;
    }

    // Method to trigger the event when the player interacts with the post-it
    public void Interact()
    {
        if (!isPostItRemoved)
        {
            isPostItRemoved = true;
            StartCoroutine(StartCutscene());
        }
    }

    // Optionally define if the player can interact with the post-it
    public bool CanInteract()
    {
        return !isPostItRemoved; // Can only interact if the post-it hasn't been removed yet
    }

    // Optionally provide interaction data, like a message to display to the player
    public IInteractable.InteractionData? GetInteractionData()
    {
        if (isPostItRemoved)
        {
            return null; // No interaction data if the post-it is already removed
        }
        return new IInteractable.InteractionData("Remove the post-it");
    }

    private IEnumerator StartCutscene()
    {
        // Start darkening the level and hide the post-it
        StartCoroutine(DarkenScene());
        gameObject.SetActive(false); // This hides the post-it itself

        // Disable player input
        inputController.enabled = false; // Disable input processing

        // Temporarily change the camera's target to focus on the enemy reveal position
        Transform originalTarget = cineVirtualCamera.Follow;
        cineVirtualCamera.Follow = enemyRevealPosition;

        // Wait for the cutscene duration
        yield return new WaitForSeconds(cutsceneDuration);

        // Reset the camera's target to follow the player
        cineVirtualCamera.Follow = originalTarget;

        // After the cutscene, re-enable player input and start enemy chase
        inputController.enabled = true; // Re-enable input processing

        enemy.GetComponent<EnemyBehavior>().StartChase(); // Assuming the enemy has an EnemyBehavior script with StartChase
    }

    private IEnumerator DarkenScene()
    {
        // Gradually darken the light over time by reducing the intensity of the Global Light 2D
        while (globalLight.intensity > 0)
        {
            globalLight.intensity -= Time.deltaTime * darkeningSpeed;
            yield return null;
        }
    }
}
