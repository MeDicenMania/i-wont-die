using UnityEngine;

public class resetPlayer : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            DeathCounter.Instance.AddDeath();
            ResetPlayer();
        }
    }

    void ResetPlayer()
    {
        Checkpoint.SendPlayersToLastCheckpoint();
    }

}
