using UnityEngine;

public class grapllepoint : MonoBehaviour
{
    [SerializeField] private float distancia;
    public Tutorial_GrapplingGun grapple;
    private GameObject player;
    private SpriteRenderer render;
    public bool canGrapple;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        distancia = Vector2.Distance(transform.position, player.transform.position);
        
        if (distancia < grapple.maxDistnace)
        {
            render.color = Color.green;
            canGrapple = true;
        }
        else
        {
            render.color= Color.gray;
            canGrapple = false;
        }

       

        
          
    }
}
