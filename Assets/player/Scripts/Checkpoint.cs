using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    static Transform lastCheckpoint;
    public Vector3 spawnOffset = new Vector3(0, 1f, 0);
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lastCheckpoint = transform;
            Debug.Log("Checkpoint saved at: " + lastCheckpoint.position);
        }
    }

    public static void SendPlayersToLastCheckpoint()
    {
        if (lastCheckpoint == null) return;
   
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.transform.position = lastCheckpoint.position + new Vector3(0, 1f, 0);

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            Debug.Log("Player " + player.name + " sent to last checkpoint at: " + player.transform.position);
        }
    }
}
