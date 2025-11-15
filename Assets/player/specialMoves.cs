using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class specialMoves : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    private movimiento mov;
    private colisiones coll;
    public Transform playerobj;
    public Transform orientation;
    [Header("Bools")]
    public bool rolling;

    [Header("Variables")]
    private float originalRadius;
    private Vector2 originalOffset;
    [SerializeField] private float rollRadius = 0.4f;
    [SerializeField] private Vector2 rollOffset = new Vector2(0f, -0.2f);

    [Header("Rolling")]
    
    private float maxRolltime;
    public float rollForce;
   
    public float rollYScale;
    private float startRollYScale;

    [Header("timers")]
    [SerializeField] private float rollDuration = 0.3f;
    public float rollTimer;
    private float momentumTimer;
    private float momentumDuration;
    private float slidecoyote = 0.10f;

    [SerializeField] private bool canRoll = true;
    [SerializeField] private bool canApplyImpulse;
    [SerializeField] private float rollSpeed;
    private bool maintainRollVelocity = false;
    private bool wasGrounded;
    private CircleCollider2D circleCol;
    private Vector2 force;
    [SerializeField] private float slidejumpForce;
    [SerializeField] private Vector2 slidejump;

    [Header("Inputs")]
    public KeyCode rollKey = KeyCode.LeftShift;
    public Vector2 dir;
    private float inputx;
    private float inputy;

    public bool isSlideJumping;

    void Start()
    {
        mov = GetComponent<movimiento>();
        coll = GetComponent<colisiones>();
        playerobj = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        startRollYScale = playerobj.localScale.y;
        circleCol = GetComponent<CircleCollider2D>();

        originalRadius = circleCol.radius;
        originalOffset = circleCol.offset;
    }


    void Update()
    {

        //timers 
        rollTimer -= Time.deltaTime;

        inputx = Input.GetAxisRaw("Horizontal");
        inputy = Input.GetAxisRaw("Vertical");
        dir = new Vector2(inputx, inputy);


        if (rolling && coll.enPiso)
        {
            //mov.conserveMomentum = true;
        }
        else if (rolling && !coll.enPiso) 
        {
            mov.conserveMomentum = false;
        }
        if (rollTimer == 0 && mov.LastOnGroundTime < 1)
        {
            canRoll = false;
        }
      
        if (coll.enPiso && !wasGrounded && (mov.playerVelocity>13 || mov.playerVelocity<-13))
        {
            canRoll = true;
        }
        wasGrounded = coll.enPiso;

        if (Input.GetKey(rollKey) && (inputx != 0))
        {
            rolling = true;
            startRoll();
        }

        if (Input.GetKeyUp(rollKey) && (rolling))
        {
            mov.conserveMomentum = false;
            isSlideJumping = false;
            rolling = false;
            stopRoll();
        }
        if (maintainRollVelocity)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                maintainRollVelocity = false;
            }
        }
        if (rolling &&  CanSlideJump() && mov.LastPressedJumpTime > 0)
        {
            mov.isJumping = false;
            mov.isJumpCut = false;
            isSlideJumping = true;

            SlideJump();
        }
    }
    private void startRoll()
    {
        Debug.Log("rolling");
  
             
        //if (rolling) return;
      
        playerobj.localScale = new Vector2(playerobj.localScale.x, rollYScale);
        circleCol.radius = rollRadius;
        circleCol.offset = rollOffset;

        if (coll.enPiso)
        {
            

        }
        if (coll.enPiso && canRoll && rolling)
        {

            
            rollTimer = rollDuration;
           
            float rollDir = Mathf.Sign(inputx);
            rollSpeed = rollDir * rollForce;
            
            canRoll = false;
            maintainRollVelocity = true;
           
        }
    }

    private void rollMovement()
    {

    }   
    private void stopRoll()
    {
        

        if (!rolling)
        {
            playerobj.localScale = new Vector2(playerobj.localScale.x, startRollYScale);
            circleCol.radius = originalRadius;
            circleCol.offset = originalOffset;
        }

    }
    private void FixedUpdate()
    {
        if (maintainRollVelocity)
        {
            rb.linearVelocity = new Vector2(rollSpeed, rb.linearVelocity.y);
        }
        if (rolling)
        {
            rollMovement();
        }

    }
    private void SlideJump()
    {


        maintainRollVelocity = false;

        Debug.Log("maxi");
      
        float dir = inputx != 0 ? Mathf.Sign(inputx) : Mathf.Sign(transform.localScale.x);
        force = new Vector2(slidejump.x * dir, slidejump.y);

        rb.AddForce(force * slidejumpForce, ForceMode2D.Impulse);
        mov.LastOnGroundTime = 0;
        mov.LastPressedJumpTime = 0;
        rolling = false;
       
    }
    private bool CanSlideJump()
    {
        return coll.enPiso && !mov.isJumping && !mov.isWallJumping && mov.wallJumpLockCounter <= 0;
    }
}


