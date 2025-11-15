using Unity.VisualScripting;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("Referencias")]
    private colisiones coll;
    private movimiento mov;
    private specialMoves spm;
    private Rigidbody2D rb;
    public KeyCode climbKey = KeyCode.E;
    private float defaultGravity;

    private float inputx;
    private float inputy;


    private int poleSide = 0; //0 centro, -1 izquierda y 1 derecha
    [SerializeField] private float poleOffset;
    [SerializeField] private float poleVerticalOffset;
    private Vector2 forceJump;
    [SerializeField] private Vector2 climbforceJump;
    [SerializeField] private float climbJumpForce;

    public bool isClimbing;
    [SerializeField] private float topClimbSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float climDownSpeed;

    void Start()
    {
        coll = GetComponent<colisiones>();
        mov = GetComponent<movimiento>();
        spm = GetComponent<specialMoves>();
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }


    void Update()
    {
        inputx = Input.GetAxisRaw("Horizontal");
        inputy = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(climbKey))
        {
            if (!isClimbing)
            {
                if (coll.onPoleH)
                {
                    ClimbHor();
                }
                else if (coll.onPole)
                {
                    Climb();
                }
            }
            else
            {
                ExitClimb();
            }
        }
        if (isClimbing)
        {
            if (coll.onPole)
            {
                Climb(); // climbing vertical
            }
            else if (coll.onPoleH)
            {
                ClimbHor(); // climbing horizontal
            }
            else
            {
                ExitClimb(); // si ya no hay pole, salir
            }
        }

    }
        
            
    private void ClimbHor()
    {
        isClimbing = true;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(inputx * climbSpeed, 0);
       
        if (Input.GetKeyDown(KeyCode.Space))
        { 
         TopClimb();
        }
           
    }
    private void Climb()
    {
        isClimbing = true;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(0, inputy * climbSpeed);
       
        if (Input.GetKeyDown(KeyCode.A))
        {
            poleSide = -1;
            SideClimb(-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            poleSide = 1;
            SideClimb(1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExitClimb();
            forceJump = new Vector2 (climbforceJump.x, climbforceJump.y);
            forceJump.x *= poleSide;

            if (Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(forceJump.x))
            {
                forceJump.x -= rb.linearVelocity.x;
            }
            if (rb.linearVelocity.y < 0)
            {
                forceJump.y -= rb.linearVelocity.y;
            }

            rb.AddForce(forceJump * climbJumpForce, ForceMode2D.Impulse);

        }

     }
    private void TopClimb()
    {
        Vector3 pos = transform.position;
        pos.y = coll.poleCenter.y + poleOffset;
        transform.position = pos;
          rb.linearVelocity = new Vector2(inputx * topClimbSpeed, 0);
    }
   private void  SideClimb(int side)
    {
        poleSide = side;

        Vector3 pos = transform.position;
        pos.x = coll.poleCenter.x + poleOffset * side;
        transform.position = pos;
        //Vector3 offset = new Vector3(poleOffset * poleSide, 0, 0);
        //transform.position = new Vector3(coll.poleCenter.x, transform.position.y, transform.position.z) + offset;

    }
    private void ExitClimb()
    {
        isClimbing = false;
        rb.gravityScale = defaultGravity;
    }
}
