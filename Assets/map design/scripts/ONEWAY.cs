using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ONEWAY : MonoBehaviour
{
    private BoxCollider2D _collider;
    private GameObject player;
    private LayerMask layer;
    [SerializeField] private bool isPlayerOnTop = true;
    [SerializeField] private float extraHeight;
    [SerializeField] private float platformTop;
    [SerializeField] private float playerBottom;
  [SerializeField]  private Vector2 distancia;
  [SerializeField]  private float distanciapapu;
  [SerializeField]  private float timer;
    [SerializeField] private float tiempito;
    private bool playerInside;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _collider = GetComponent<BoxCollider2D>();
        

    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
        platformTop = _collider.bounds.max.y;


        playerBottom = player.GetComponent<CapsuleCollider2D>().bounds.min.y;

        isPlayerOnTop = playerBottom > platformTop + extraHeight;
        

         distancia = _collider.ClosestPoint(player.transform.position);

        if (playerInside && (Input.GetKey(KeyCode.S)))
        {
          
                Debug.Log("PARA ABAJO PAPI");
               _collider.isTrigger = true;
                gameObject.layer = LayerMask.NameToLayer("UI");
                timer = tiempito;
                return;
        }

        if( timer > 0)
        {
            _collider.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("ground");
        }

            if (isPlayerOnTop)
            {
                _collider.isTrigger = false;
                gameObject.layer = LayerMask.NameToLayer("ground");
            }
            else 
            {
                _collider.isTrigger = true;
                gameObject.layer = LayerMask.NameToLayer("UI");
            }
        }

   


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            playerInside = true;


            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}




    
