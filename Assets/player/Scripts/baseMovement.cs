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


    private float lastWallJumpDir;
    private float moveInputx;
    private float moveInputy;

    [Header("Estados")]
    [HideInInspector] public bool isClimbing;
    [HideInInspector] public bool jump;
    public bool isJumping;
    [HideInInspector] public bool wallJumped;
    public bool isWallJumping;
     public bool isSliding;
    [HideInInspector] private bool isFacingRight = true;
    [HideInInspector] public bool isJumpFalling;
    [HideInInspector] public bool isJumpCut;
    public bool isSpeeding;
    [Header("Referencias")]
        private Climbing Climbing;
        private Rigidbody2D rb;
        private LayerMask ground;
        private colisiones coll;
        private specialMoves sm;
        private GameObject spawn;
        private GameObject cops;
        private GameObject win;
        private Animator animator;
    private Tutorial_GrapplingGun grapple;
    [SerializeField] PlayerData1 playerData;

        [Header("Variables")]
    public float playerVelocity;
    [SerializeField] private float lastImageEx;
    [SerializeField] private float distanceAfter;
    private float targetSpeed;
    private float accelRate;
    private float defaultGravity;
    private float movement;
    private Vector2 force;
    private int Dir;
    private PlayerSounds playerSounds;
    void Start()
        {
        playerSounds  = GetComponent<PlayerSounds>();
        grapple = GetComponentInChildren<Tutorial_GrapplingGun>();
            Climbing = GetComponent<Climbing>();
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<colisiones>();
           sm = GetComponent<specialMoves>();
            spawn = GameObject.FindWithTag("spawn");
            cops = GameObject.FindWithTag("cops");
            GameObject enemyGO = GameObject.Find("enemy");
            win = GameObject.FindWithTag("win");
            defaultGravity = rb.gravityScale;
            animator= GetComponent<Animator>();
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

            if(playerVelocity >14  || playerVelocity < -14)
            {
                isSpeeding = true;
            }
            else
            {
                isSpeeding = false;
            }

            //chequeos 
            if (coll.enPiso)
                LastOnGroundTime = playerData.coyote;
        
            if (coll.onRightWall)
                LastOnWallRightTime = playerData.coyote;
        
            if (coll.onLeftWall)
                LastOnWallLeftTime = playerData.coyote;

            if (coll.enPared && !coll.enPiso && !isWallJumping)
            {
                WallSlide();

            }

            //colision piso
            if ( (Input.GetKeyDown(KeyCode.Space)))
            {

                LastPressedJumpTime = playerData.jumpBuffer;
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
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (playerData.fallmultiplier - 1) * Time.deltaTime;
            }
            else if (rb.linearVelocityY > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (playerData.lowjumpmultiplier - 1) * Time.deltaTime;
            }
       
            if(coll.enPiso || Climbing.isClimbing)
            {
                playerData.wallJumpForce = new Vector2(10f, 10f);
                lastWallJumpDir = 0;
            }
        

           
            //jump cheks

            if (isJumping && rb.linearVelocity.y < 0)
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
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
        if (moveInputx > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInputx < 0 && isFacingRight)
        {
            Flip();
        }

        if (CanJump() && LastPressedJumpTime > 0)
            {
            
                isJumping = true;
                isWallJumping = false;
                isJumpCut = false;
                isJumpFalling = false;
                animator.SetBool("isJumping", true);
                
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
                playerData.maxVelocity = playerData.initialSpeed;
            }

       

       
            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        if (isSpeeding)
        {
            Pool.Instance.GetFromPool();
            lastImageEx = transform.position.x;

            if (Mathf.Abs(transform.position.x - lastImageEx) > distanceAfter)
            {
                Pool.Instance.GetFromPool();
                lastImageEx = transform.position.x;
            }

        }
        ActualizarAnimaciones();
        }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);

    }
        public void WallJump(int Dir)
        {
       
            isWallJumping = true;
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;

            wallJumpStartTime = 0;

            wallJumpLockCounter = wallJumpLockTime;




            force = new Vector2(playerData.wallJumpForce.x, playerData.wallJumpForce.y);

            force.x *= Dir;

            if( lastWallJumpDir == Dir)
            {
            playerData.wallJumpForce.x = playerData.wallJumpForce.x / 2;
            playerData.wallJumpForce.y = playerData.wallJumpForce.y / 2;
            }
            else
            {
            playerData.wallJumpForce.x = 10; playerData.wallJumpForce.y = 10;
            }

            if (Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(force.x))
            {
                force.x -= rb.linearVelocity.x;
            }


            if (rb.linearVelocity.y < 0)
            {
                force.y -= rb.linearVelocity.y;
            }


            rb.AddForce(force * playerData.WallJumpmult, ForceMode2D.Impulse);
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
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, -playerData.walllSlideSpeed);

                }
           
  
            }
            

        }
        private void Jump()
        {
            Debug.Log("saltooo");
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;

            rb.AddForce(Vector2.up * playerData.jumpForce, ForceMode2D.Impulse);

            isJumping = true;
       
        }
      
    
        private void Run(float lerp)
        {
            //Debug.Log("running");
            isSliding = false;

            if (CanRun())
            {
            
                 targetSpeed = moveInputx * playerData.maxVelocity;

                targetSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, lerp);

                float speedDiff = targetSpeed - rb.linearVelocity.x;
          

                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccelAmount : playerData.runDecclAmount;

            
         
                movement = speedDiff * accelRate;

                if (LastOnGroundTime > 0)
                    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccelAmount : playerData.runDecclAmount;
                else
                    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccelAmount * playerData.accelAair : playerData.runDecclAmount * playerData.deccAir;

                rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

                playerVelocity = rb.linearVelocity.x ;
            } 
        

        }
        private void FixedUpdate()
        {

            if ((!coll.enPared || coll.enPiso) && !Climbing.isClimbing && !grapple.isDashing && !grapple.isGrabing)
            {
                Run(1);
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
    
    
        
       

       
        void ActualizarAnimaciones()
        {
            bool grounded = coll.enPiso;
            bool moving = Mathf.Abs(moveInputx) > 0.1f;

            // IDLE
            if(sm.rolling)
        {
            animator.SetBool("isSliding", true);

        }
        else
        {
            animator.SetBool("isSliding", false);
        }
            if (grounded && !moving)
        {
            animator.SetBool("IsRunning", false);
            playerSounds.HandleFootsteps(false);
            animator.SetBool("isIdle", true);
        }
        // RUN
        else if (grounded && moving)
        {
            animator.SetBool("IsRunning", true);
            playerSounds.HandleFootsteps(true);
            animator.SetBool("isIdle", false);
        }
        // AIRE
        else
        {
            animator.SetBool("IsRunning", false);
            playerSounds.HandleFootsteps(false);
            animator.SetBool("isIdle", false);
        }

            // JUMP
            animator.SetBool("isJumping", isJumping);
            playerSounds.PlayJump();


        // WALL SLIDE
        animator.SetBool("isWallSliding", isSliding);
        playerSounds.SetWallSlide(isSliding);
        }





    }

