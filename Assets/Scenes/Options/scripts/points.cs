using UnityEngine;

public class PointObject : MonoBehaviour
{
    public int pointValue = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PointsCounter.Instance.AddPoints(pointValue); 
            Destroy(gameObject);
        }
    }
}
