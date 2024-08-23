using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject debrisPrefab;  // The debris object to spawn
    [SerializeField] private float spawnInterval = 5f;  // Time interval between spawns, exposed to inspector

    private float timeSinceLastSpawn;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnDebris();
            timeSinceLastSpawn = 0f;  // Reset the timer after spawning
        }
    }

    private void SpawnDebris()
    {
        if (debrisPrefab != null)
        {
            // Spawn the debris at the spawner's position and rotation
            Instantiate(debrisPrefab, transform.position, transform.rotation);
            Debug.Log("Debris spawned at " + transform.position);
        }
        else
        {
            Debug.LogWarning("Debris prefab is not assigned!");
        }
    }
}
