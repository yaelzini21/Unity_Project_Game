using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefab;
    public float spawnInterval = 20f;

    private GameObject portalA, portalB;

    void Start()
    {
        InvokeRepeating("SpawnPortals", 5f, spawnInterval);
    }

    void SpawnPortals()
    {
        if (portalA) Destroy(portalA);
        if (portalB) Destroy(portalB);

        Vector3 posA = GetRandomPortalPosition();
        Vector3 posB;

        int maxTries = 20;
        int tries = 0;
        float minDistance = 5f;

        do
        {
            posB = GetRandomPortalPosition();
            tries++;
        } while (Vector3.Distance(posA, posB) < minDistance && tries < maxTries);

        portalA = Instantiate(portalPrefab, posA, Quaternion.identity);
        portalB = Instantiate(portalPrefab, posB, Quaternion.identity);

        portalA.GetComponent<PortalController>().linkedPortal = portalB.transform;
        portalB.GetComponent<PortalController>().linkedPortal = portalA.transform;

        // Destroy after 3 seconds
        Destroy(portalA, 3f);
        Destroy(portalB, 3f);
    }

    private Vector3 GetRandomPortalPosition()
    {
        float safeDistanceFromPlayers = 4f;
        float minX = -13f + safeDistanceFromPlayers;
        float maxX = 13f - safeDistanceFromPlayers;

        float x = Random.Range(minX, maxX);
        float z = Random.Range(-5f, 5f);

        return new Vector3(x, 0.5f, z);
    }


}
