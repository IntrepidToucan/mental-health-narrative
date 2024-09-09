using Interaction;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RopeInteraction : MonoBehaviour, IInteractable
{
    public enum InteractionType { Fall, Wither }

    [SerializeField] private InteractionType interactionType;  // Choose between falling or withering
    [SerializeField] private GameObject targetObject;  // The object that will fall or wither
    [SerializeField] private float fallSpeed = 5f;  // Speed of the object when it falls
    [SerializeField] private float witherDuration = 2f;  // Duration for withering effect

    private bool isTriggered = false;

    // IInteractable method to define interaction prompt
    public bool CanInteract()
    {
        return !isTriggered;  // Can interact only if the interaction hasn't been triggered yet
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        return new IInteractable.InteractionData("Press 'E' to pull the rope");
    }

    // IInteractable method to handle interaction
    public void Interact()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            if (interactionType == InteractionType.Fall)
            {
                TriggerFall();
            }
            else if (interactionType == InteractionType.Wither)
            {
                StartCoroutine(TriggerWither());
            }
        }
    }

    // Causes the target object to fall
    private void TriggerFall()
    {
        if (targetObject != null)
        {
            Rigidbody2D rb = targetObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;  // Make the object fall by turning on physics
                rb.velocity = new Vector2(0, -fallSpeed);  // Apply falling speed
            }
            else
            {
                Debug.LogError("Target object needs a Rigidbody2D to fall.");
            }
        }
    }

    // Causes the target object to wither (fade out and disappear)
    private IEnumerator TriggerWither()
    {
        if (targetObject != null)
        {
            SpriteRenderer sr = targetObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color originalColor = sr.color;
                float elapsedTime = 0f;

                while (elapsedTime < witherDuration)
                {
                    elapsedTime += Time.deltaTime;
                    sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, elapsedTime / witherDuration));
                    yield return null;
                }

                Destroy(targetObject);  // Remove the object once it has fully withered away
            }
            else
            {
                Debug.LogError("Target object needs a SpriteRenderer to wither.");
            }
        }
    }
}
