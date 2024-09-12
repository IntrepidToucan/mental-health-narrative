using UnityEngine;
using Interaction;
using System.Collections;
using System.Collections.Generic;

public class BranchLowering : MonoBehaviour
{
    [SerializeField] private Transform branch;        // The branch that will lower
    [SerializeField] private Vector3 loweringOffset;  // How much to lower the branch by
    [SerializeField] private float loweringSpeed = 2f;  // Speed of lowering the branch
    [SerializeField] private string boxTag = "Box";   // Tag to identify the box that triggers lowering

    private bool isLowering = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the box
        if (collision.gameObject.CompareTag(boxTag) && !isLowering)
        {
            // Start lowering the branch
            StartCoroutine(LowerBranch());
        }
    }

    private IEnumerator LowerBranch()
    {
        isLowering = true;
        Vector3 initialPosition = branch.position;
        Vector3 targetPosition = initialPosition + loweringOffset;

        // Gradually lower the branch over time
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            branch.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * loweringSpeed;
            yield return null;
        }

        // Ensure the branch ends at the exact target position
        branch.position = targetPosition;

        Debug.Log("Branch has been lowered!");
        isLowering = false;
    }
}
