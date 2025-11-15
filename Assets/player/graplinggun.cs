using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;
    public movimiento mov;
    public colisiones coll;
    [Header("Layers Settings:")]
    [SerializeField] private int grappableLayerNumber = 9;

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

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        allGrapplePoints = GameObject.FindGameObjectsWithTag("grapplepoint");
    }

    private void Update()
    {
      

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }

   

        if (Vector2.Distance(rb.position, dashTarget) < launchSpeed || dashTimer <= 0)
        {
            StopDash();
            Debug.Log("Dash completado al punto del gancho");
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
           
        }
        else
        {
            Debug.Log("No hay ningún punto de agarre dentro del rango.");
        }
    }

    public void Grapple()
    {
        
        m_springJoint2D.autoConfigureDistance = false;
        if (launchToPoint)
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    {
                        m_springJoint2D.enabled = true;
                        m_springJoint2D.connectedAnchor = grapplePoint;
                        float dist = Vector2.Distance(transform.position, grapplePoint);
                        m_springJoint2D.distance = dist * 0.7f;
                        m_springJoint2D.frequency = launchSpeed;
                        m_springJoint2D.dampingRatio = 0.3f;

                        break;
                    }
                case LaunchType.Transform_Launch:
                    {
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
            float forceMagnitude = Mathf.MoveTowards(dashSpeed, dash, Time.deltaTime * speedBuildUp);
            rb.AddForce(dir * forceMagnitude, ForceMode2D.Force);
        }
    }
    private void StopDash()
    {

        dashTimer = 1;
        isDashing = false;
        rb.gravityScale = 3;
        //mov.conserveMomentum = true;
        grappleRope.enabled = false;
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
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

}
