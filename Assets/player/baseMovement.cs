using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    //timers
    [HideInInspector] public float LastOnGroundTime;
    [HideInInspector] private float LastOnWallTime;
    [HideInInspector] private float LastOnWallRightTime;
    [HideInInspector] private float LastOnWallLeftTime;
    [HideInInspector] public float LastPressedJumpTime;

    [SerializeField] private float wallJumpStartTime;
    [SerializeField] private float wallJumpTime = 0.25f;

    [SerializeField] private float wallJumpLockTime = 0.15f;
    [HideInInspector] public float wallJumpLockCounter;

    [Header("Referencias")]
    private Climbing Climbing;
    private Rigidbody2D rb;
    private LayerMask ground;
    private colisiones coll;
    private specialMoves sm;
    private GameObject spawn;
    private GameObject cops;
    private GameObject win;
    [SerializeField] private death death;
    [Header("Variables")]
    private float defaultGravity;

    private float coyote = 0.12f;
    public float playerVelocity;
    private float jumpBufferTime = 0.12f;
   //air Movement
    [SerializeField] private float accelAair;
    [SerializeField] private float deccAir;

    //Jump
    [SerializeField] private float fallmultiplier = 2.5f;
    [SerializeField] private float lowjumpmultiplier = 2f;
    [SerializeField] private float forceJump = 2.5f;
    [HideInInspector] public  bool conserveMomentum = false;

    //Movimiento
    public bool isSpeeding;
    public float flipSpeed;
    private float targetSpeed;
    private float accelRate;
    [SerializeField] private float baseSpeed = 13;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float speedBuildUp;
    [SerializeField] private float velocity;
    private float movement;
    private float moveInputx;
    private float moveInputy;
    [SerializeField] private float runAccelAmount;
    [SerializeField] private float runDecclAmount;
    //Vector2
    private Vector2 dir;
    private Vector2 force;
    //Waljump 
    [SerializeField] private float wallforceJump;

    [SerializeField] private Vector2 wallJumpForce;
    private float lastWallJumpDir;
    private float slideSpeed = 2.5f;
    private int Dir;

    [Header("Bools")]
    [HideInInspector] public bool isClimbing;
    [HideInInspector] public bool isJumpFalling;
    [HideInInspector] public bool isJumpCut;
    [HideInInspector] public bool jump;
     public bool isJumping;
    [HideInInspector] public bool wallJumped;
     public bool isWallJumping;
    [HideInInspector] public bool isSliding;
    [HideInInspector] private bool isFacingRight = true;
    
    
    void Start()
    {
        Climbing = GetComponent<Climbing>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<colisiones>();
       sm = GetComponent<specialMoves>();
        spawn = GameObject.FindWithTag("spawn");
        cops = GameObject.FindWithTag("cops");
        GameObject enemyGO = GameObject.Find("enemy");
        win = GameObject.FindWithTag("win");
        defaultGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {

        //timers
        wallJumpLockCounter -= Time.deltaTime;
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        wallJumpStartTime += Time.deltaTime;

        //inputs
        moveInputx = Input.GetAxisRaw("Horizontal");
        moveInputy = Input.GetAxisRaw("Vertical");


        Vector2 dir = new Vector2(moveInputx, moveInputy);

        if(playerVelocity >13  || playerVelocity < -13)
        {
            isSpeeding = true;
        }
        else
        {
            isSpeeding = false;
        }

        //chequeos 
        if (coll.enPiso)
            LastOnGroundTime = coyote;
        
        if (coll.onRightWall)
            LastOnWallRightTime = coyote;
        
        if (coll.onLeftWall)
            LastOnWallLeftTime = coyote;

        if (coll.enPared && !coll.enPiso && !isWallJumping)
        {
            WallSlide();

        }

        //colision piso
        if ( (Input.GetKeyDown(KeyCode.Space)))
        {

            LastPressedJumpTime = jumpBufferTime;
            isSliding = false;
            


        }
        //colision paredes
        if (coll.enPared && (coll.enPiso == false) && (Input.GetKeyDown(KeyCode.Space)) )
        {
            isSliding = false;
        }
        //jump cut

        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallmultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowjumpmultiplier - 1) * Time.deltaTime;
        }
       
        if(coll.enPiso || Climbing.isClimbing)
        {
            wallJumpForce = new Vector2(10f, 10f);
            lastWallJumpDir = 0;
        }
        

        if (moveInputx >0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInputx < 0 && isFacingRight)
        {
            Flip(); 
        }
        //jump cheks

        if (isJumping && rb.linearVelocity.y < 0)
        {
            isJumping = false;
        }
       
        if (!isWallJumping)
        {
            isJumpFalling = true;

        }
            
        if (isWallJumping && wallJumpStartTime > wallJumpTime)
        {
            isWallJumping = false;
        }
        if (!isJumping)
        {
            isJumpFalling = false;
        }
           

        if (CanJump() && LastPressedJumpTime > 0)
        {
            isJumping = true;
            isWallJumping = false;
            isJumpCut = false;
            isJumpFalling = false;
            Jump();
        }
        // walljump cheks
        else if (CanWallJump() && LastPressedJumpTime > 0)
        {
           
            isJumping = false;
            isJumpCut = false;
            isJumpFalling = false;
            wallJumpStartTime = Time.time;

          
           

             Dir = coll.onRightWall ? -1 : 1; WallJump(Dir);
            
        }
        if (coll.enPared)
        {
            velocity = baseSpeed;
        }

       

       
        LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);


    }


    public void WallJump(int Dir)
    {
       
        isWallJumping = true;
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        wallJumpStartTime = 0;

        wallJumpLockCounter = wallJumpLockTime;




        force = new Vector2(wallJumpForce.x, wallJumpForce.y);

        force.x *= Dir;

        if( lastWallJumpDir == Dir)
        {
            wallJumpForce.x = wallJumpForce.x / 2;
            wallJumpForce.y = wallJumpForce.y / 2;
        }
        else
        {
            wallJumpForce.x = 10; wallJumpForce.y = 10;
        }

        if (Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(force.x))
        {
            force.x -= rb.linearVelocity.x;
        }


        if (rb.linearVelocity.y < 0)
        {
            force.y -= rb.linearVelocity.y;
        }


        rb.AddForce(force * wallforceJump, ForceMode2D.Impulse);
        lastWallJumpDir = Dir;
    }
    private  void TimewallJumping()
    {
        isWallJumping = false;
    }

    public void WallSlide() 
    {
        isSliding = true;

        if (!isWallJumping)
        {
            //rb.AddForce(Vector2.up * (velocity), ForceMode2D.Force);
                   if (coll.wallSide != moveInputx || moveInputx == 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -slideSpeed);

            }
           
  
        }
            

    }
    private void Jump()
    {
        Debug.Log("saltooo");
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        rb.AddForce(Vector2.up * forceJump, ForceMode2D.Impulse);

        isJumping = true;
       
    }
      
    
    private void Run(float lerp)
    {
        //Debug.Log("running");
        isSliding = false;

        if (CanRun())
        {
             targetSpeed = moveInputx * velocity;

            if (moveInputx != 0)
            {
                velocity = Mathf.MoveTowards(velocity, maxVelocity, Time.deltaTime * speedBuildUp);
            }
     
            targetSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, lerp);

            float speedDiff = targetSpeed - rb.linearVelocity.x;
          

            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDecclAmount;

            
         
            movement = speedDiff * accelRate;

            if (LastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDecclAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelAair : runDecclAmount * deccAir;

            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

            if (conserveMomentum && Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
            {
                accelRate = 0;
            }


            playerVelocity = rb.linearVelocity.x ;
        } 
        

    }

    private void Momemtum()
    {
       

          
    }


    private void FixedUpdate()
    {

        if ((!coll.enPared || coll.enPiso) && !Climbing.isClimbing)
        {
            Run(1);
        }
    }

    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        if(coll.enPiso && isSpeeding)
        {
            rb.linearVelocity = new Vector2((flipSpeed) * moveInputx, rb.linearVelocity.y);
        }
     

    }
    public bool CanJump()
    {
        return LastOnGroundTime > 0 && !isJumping && !isWallJumping && wallJumpLockCounter <= 0 && !sm.rolling;
    }
    private bool CanWallJump()
    {
        return wallJumpLockCounter <= 0.15f && LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!isWallJumping ||
             (LastOnWallRightTime > 0 && lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && lastWallJumpDir == -1));
    }
    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !isJumping && !isWallJumping && LastOnGroundTime <= 0)
            return true;
        else
            return false;
    }
    public bool CanRun()
    {
       if  (!isWallJumping && !sm.isSlideJumping)
        return true;
            else 
        return false;
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("spawn"))
        {
            death.spawn = true;
        }
            if (collision.gameObject.CompareTag("cops"))
        {
            Respawn();
        }
            if ( collision.gameObject.CompareTag("win"))
        {
            SceneManager.LoadScene("Main Menu");
        }
        
    }
    private void Respawn()
    {
        //SceneManager.LoadScene("Main Menu");
        Debug.Log("perdiste jaja");
    }    
}

