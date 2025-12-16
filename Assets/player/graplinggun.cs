using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;
    public movimiento mov;
    public colisiones coll;
<<<<<<< Updated upstream:Assets/player/graplinggun.cs
    [Header("Layers Settings:")]
    [SerializeField] private int grappableLayerNumber = 9;
=======
  [SerializeField]  private PlayerSounds playerSound;
    [SerializeField] PlayerData1 playerData;
    [Header("Layers Settings:")]
>>>>>>> Stashed changes:Assets/player/Scripts/graplinggun.cs

    public bool gizmover;
    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D rb;

    private Vector2 force;


    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
     public float maxDistnace;
    private Vector2 targetPos;
    [SerializeField] private float dashSpeed;
    [Header("Grapplepoint")]
    [SerializeField] private float dashTimer;
    [SerializeField] private float dashTime = 5f;
    [SerializeField] private float dash;
    [SerializeField] private float speedBuildUp;
    public  bool isDashing = false;
    private Vector2 dashTarget;
    private GameObject[] allGrapplePoints;
    public  bool isGrabing;
   [SerializeField]  private int exitGrabSide;
    private Vector2 forceJump;
 [SerializeField]   private bool hayObstaculo;
   [SerializeField] private LayerMask obstacleMask;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float minDistance;

<<<<<<< Updated upstream:Assets/player/graplinggun.cs
    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;
=======
   
>>>>>>> Stashed changes:Assets/player/Scripts/graplinggun.cs

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        playerSound = GetComponentInParent<PlayerSounds>();
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        allGrapplePoints = GameObject.FindGameObjectsWithTag("grapplepoint");
    }

    private void Update()
    {
        ActualizarRaycastObstaculo();

        if (!hayObstaculo)
          {


            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SetGrapplePoint();
            }
        }


        if (  isDashing && Vector2.Distance(rb.position, dashTarget) < minDistance)
        {
            StopDash();
            Debug.Log("Dash completado al punto del gancho");
        }
        if(Vector2.Distance(rb.position, dashTarget)> minDistance)
        {
            ExitGrab();
        }
        if ((Input.GetKeyDown(KeyCode.D)))
        {
            exitGrabSide = 1;
        }
        if ((Input.GetKeyDown(KeyCode.A)))
        {
            exitGrabSide = -1;
        }

        if (isGrabing)
        {
           
            if (Input.GetKeyDown(KeyCode.Space))
            {
                forceJump = new Vector2(playerData.wallJumpForce.x, playerData.wallJumpForce.y);
                forceJump.x *= exitGrabSide;

                if (Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(forceJump.x))
                {
                    forceJump.x -= rb.linearVelocity.x;
                }
                if (rb.linearVelocity.y < 0)
                {
                    forceJump.y -= rb.linearVelocity.y;
                }

                rb.AddForce(forceJump * playerData.WallJumpmult, ForceMode2D.Impulse);

                ExitGrab();
            }


        }
        if(hayObstaculo)
        {
            isDashing = false;
            grappleRope.enabled = false; 
        }
    }

   private void ActualizarRaycastObstaculo()
    {
        if (grapplePoint != Vector2.zero)
        {
            Transform nearestPoint = GetNearestGrapplePoint(firePoint.position);

            if (nearestPoint != null)
            {
                grapplePoint = nearestPoint.position;
                grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
              
            }


            Vector2 dir = grapplePoint - (Vector2)rb.position;
            float dist = dir.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(rb.position, dir.normalized, dist, obstacleMask);

            hayObstaculo = hit.collider != null;

            Debug.DrawLine(rb.position, grapplePoint, hayObstaculo ? Color.red : Color.green);
        }
        else
        {
            hayObstaculo = false;
        }
    
    }

    void SetGrapplePoint()
    {
        Transform nearestPoint = GetNearestGrapplePoint(firePoint.position);

        if (nearestPoint != null)
        {
            grapplePoint = nearestPoint.position;
            grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
            grappleRope.enabled = true;
            ActualizarRaycastObstaculo();
        }
        else
        {
            Debug.Log("No hay ningún punto de agarre dentro del rango.");
        }
    }

    public void Grapple()
    {
        if (hayObstaculo)
        {
            return;
        }
            m_springJoint2D.autoConfigureDistance = false;

            if (launchToPoint)
            {
                switch (launchType)
                {

                    case LaunchType.Transform_Launch:
                        {
                        playerSound.PlayHook();
                            rb.gravityScale = 1;
                            rb.linearVelocity = Vector2.zero;
                            dashTimer = dashTime;
                            isDashing = true;
                            dashTarget = grapplePoint;
                            break;
                        }
                }

            }
        
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;


            Vector2 dir = (dashTarget - rb.position).normalized;
<<<<<<< Updated upstream:Assets/player/graplinggun.cs
            float forceMagnitude = Mathf.MoveTowards(dashSpeed, dash, Time.deltaTime * speedBuildUp);
            rb.AddForce(dir * forceMagnitude, ForceMode2D.Force);
=======
            rb.linearVelocity = dir * dashSpeed;



            
>>>>>>> Stashed changes:Assets/player/Scripts/graplinggun.cs
        }
    }
    private void StopDash()
    {

        dashTimer = 1;
        isDashing = false;
        Grab();
        grappleRope.enabled = false;
        isGrabing = true;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }

    private void Grab()
    {
       
       
        
    }
    private void ExitGrab()
    {
        isGrabing = false;
        rb.gravityScale = 3;
    }
    private Transform GetNearestGrapplePoint(Vector2 fromPosition)
    {
        Transform nearest = null;
        float minDist = maxDistnace;

        foreach (GameObject gp in GameObject.FindGameObjectsWithTag("grapplepoint"))
        {
            float dist = Vector2.Distance(fromPosition, gp.transform.position);
            if (dist <= minDist)
            {
                minDist = dist;
                nearest = gp.transform;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        if (gizmover)
        {

            if (firePoint != null && hasMaxDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
            }
        }
    }
}
