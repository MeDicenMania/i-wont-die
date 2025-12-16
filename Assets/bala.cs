using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class bala : MonoBehaviour
{
    private GameObject Player;
    private Vector2 direccion;
    private float distance;
    private GameObject death;
    private death target;
    [SerializeField] private float Velocidad = 4;
    private float timer;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        death = GameObject.FindWithTag("Target");
        target = death.GetComponent<death>();
        direccion = (Player.transform.position - transform.position).normalized;



    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, Player.transform.position);

        timer += Time.deltaTime;

        transform.position += (Vector3)direccion * Velocidad * Time.deltaTime;

        

        if (timer > 3)
        {
            Destroy(gameObject);
        }
        if(target.resetPlayer)
        {
            Destroy(gameObject);
        
        }
        

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
     if(collision.CompareTag("Player"))
        {
            target.ResetState();
            Checkpoint.SendPlayersToLastCheckpoint();
            if (DetectionManager.Instance != null)
                DetectionManager.Instance.ResetGlobalAlert();
            Destroy(gameObject);
        }
        
    }
}
