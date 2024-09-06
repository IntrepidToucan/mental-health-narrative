using UnityEngine;
using UnityEngine.Rendering.Universal;
using Interaction;
using System.Collections;
using System.Collections.Generic;
public class MainRoom_CrowdTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject sunPainting;  // The sun painting object
    [SerializeField] private GameObject crowd;  // The crowd of people object
    [SerializeField] private Light2D globalLight;  // The 2D global light in the room
    [SerializeField] private GameObject realDomainScenery;  // Mandy's true domain scenery
    [SerializeField] private GameObject artMuseumScenery;  // Original art museum scenery
    [SerializeField] private float fadeDuration = 2f;  // How long the light fade takes

    private bool isTriggered = false;

    // This allows the player to interact with the painting if it hasn't been triggered yet
    public bool CanInteract()
    {
        return !isTriggered;
    }

    // Shows an interaction prompt when the player is near the painting
    public IInteractable.InteractionData? GetInteractionData()
    {
        return new IInteractable.InteractionData("Press 'E' to remove the painting");
    }

    // This is called when the player interacts with the painting
    public void Interact()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            RemovePainting();
        }
    }

    // The method that removes the painting, scatters the crowd, darkens the room, and changes the scenery
    private void RemovePainting()
    {
        if (sunPainting != null) Destroy(sunPainting);  // Remove the painting
        if (crowd != null) Destroy(crowd);  // Scatter the crowd

        // Start the room darkening
        if (globalLight != null)
        {
            StartCoroutine(DarkenRoom());
        }

        // Switch the scenery
        ChangeScenery();
    }

    // Coroutine to gradually darken the room by lowering the global light intensity
    private IEnumerator DarkenRoom()
    {
        float initialIntensity = globalLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            globalLight.intensity = Mathf.Lerp(initialIntensity, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        globalLight.intensity = 0f;
    }

    // Method to change the scenery after the painting is removed
    private void ChangeScenery()
    {
        if (artMuseumScenery != null) artMuseumScenery.SetActive(false);  // Hide the art museum scenery
        if (realDomainScenery != null) realDomainScenery.SetActive(true);  // Reveal Mandy's true domain
    }
}