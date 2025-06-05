using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform leftPlayer;
    public Transform rightPlayer;
    public float minSpawnInterval = 15f;
    public float maxSpawnInterval = 25f;
    public float minBubbleDuration = 1f;
    public float maxBubbleDuration = 3f;

    private float nextSpawnTime;
    private float spawnSafetyMargin = 4.5f;

    void Start()
    {
        ScheduleNextBubble();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnBubble();
            ScheduleNextBubble();
        }
    }

    void ScheduleNextBubble()
    {
        float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
        nextSpawnTime = Time.time + interval;
    }

        void SpawnBubble()
    {
        Transform randomPlayer = Random.value < 0.5f ? leftPlayer : rightPlayer;
        float x = randomPlayer.position.x;

        float z = Random.value < 0.5f && randomPlayer.position.z - spawnSafetyMargin > -7 
            ? Random.Range(-7f, randomPlayer.position.z - spawnSafetyMargin) 
            : Random.Range(randomPlayer.position.z + spawnSafetyMargin, 7f);

        Vector3 spawnPos = new Vector3(x, 0.5f, z);
        
        GameObject bubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);

        float duration = Random.Range(minBubbleDuration, maxBubbleDuration);

        Destroy(bubble, duration); // ✅ השתמשי כאן באותו duration שהגדרת
    }

}
