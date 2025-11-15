using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    bool spawned = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!spawned && other.CompareTag("Player"))
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawned = true;
        }
    }
}
