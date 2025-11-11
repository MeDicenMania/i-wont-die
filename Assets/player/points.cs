using UnityEngine;

public class points : MonoBehaviour
{
    public int pointValue = 1;
    public PointsCounter pointsCounter;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pointsCounter.AddPoints(pointValue);
            Destroy(gameObject);
        }
    }
}
