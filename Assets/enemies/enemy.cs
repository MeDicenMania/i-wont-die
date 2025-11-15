using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float accuracy = 0f;
    private Vector3 velocity;

    void Update()
    {
        float acceleration = accuracy * 0.25f;

        Vector3 dir = (player.position - transform.position).normalized;
        float targetSpeed = speed * accuracy;

        velocity = Vector3.MoveTowards(velocity, dir * targetSpeed, acceleration * Time.deltaTime);
        transform.position += velocity * Time.deltaTime;

        if (accuracy < 0.2f)
        {
            transform.position += transform.right * Mathf.Sin(Time.time * 2f) * 0.1f;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < 0.5f)
        {
            //Checkpoint.SendPlayerToLastCheckpoint(player);
            ResetState();
        }
    }

    public void GainAccuracy(float amt)
    {
        accuracy += amt;
        accuracy = Mathf.Clamp01(accuracy);
    }

    void ResetState()
    {
        accuracy = 0f;
        velocity = Vector3.zero;
        transform.position = transform.position; // stays put
    }
}


