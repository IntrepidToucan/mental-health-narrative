using UnityEngine;
using System.Collections;

public class PlatformExplosion : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private Transform[] platforms; // All platform pieces forming the floor
    [SerializeField] private Vector3[] targetPositions; // The exact world positions of the platforms after the explosion
    [SerializeField] private Vector3[] targetScales; // The exact scales of the platforms after the explosion
    [SerializeField] private Vector3[] targetRotations; // The exact rotations of the platforms after the explosion (Euler angles)

    private Vector3[] initialPositions;
    private Vector3[] initialScales;
    private Quaternion[] initialRotations;

    private void Start()
    {
        StoreInitialTransforms();
    }

    private void StoreInitialTransforms()
    {
        initialPositions = new Vector3[platforms.Length];
        initialScales = new Vector3[platforms.Length];
        initialRotations = new Quaternion[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            initialPositions[i] = platforms[i].position;
            initialScales[i] = platforms[i].localScale;
            initialRotations[i] = platforms[i].rotation;
        }
    }

    public void CaptureTargetTransforms()
    {
        targetPositions = new Vector3[platforms.Length];
        targetScales = new Vector3[platforms.Length];
        targetRotations = new Vector3[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            targetPositions[i] = platforms[i].position;
            targetScales[i] = platforms[i].localScale;
            targetRotations[i] = platforms[i].rotation.eulerAngles;
        }
    }

    public void ResetToInitialPositions()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].position = initialPositions[i];
            platforms[i].localScale = initialScales[i];
            platforms[i].rotation = initialRotations[i];
        }
    }

    public void TriggerExplosion()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            StartCoroutine(TransformPlatform(platforms[i], targetPositions[i], targetScales[i], Quaternion.Euler(targetRotations[i])));
        }
    }

    private IEnumerator TransformPlatform(Transform platform, Vector3 targetPosition, Vector3 targetScale, Quaternion targetRotation)
    {
        Vector3 startPosition = platform.position;
        Vector3 startScale = platform.localScale;
        Quaternion startRotation = platform.rotation;
        float elapsedTime = 0f;
        float explosionDuration = 1f; // Duration of the explosion animation

        while (elapsedTime < explosionDuration)
        {
            float t = elapsedTime / explosionDuration;
            platform.position = Vector3.Lerp(startPosition, targetPosition, t);
            platform.localScale = Vector3.Lerp(startScale, targetScale, t);
            platform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        platform.position = targetPosition;
        platform.localScale = targetScale;
        platform.rotation = targetRotation;
    }
}
