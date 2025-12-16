using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.UI;
public class OscillatorCamera : MonoBehaviour
{


    [Header("References")]
   [SerializeField]  private  DeteccionSystem deteccionSystem;
    private GameObject target;
    [SerializeField] private Transform pivotTransform;

    [SerializeField] private GameObject player;
    private death death;
    [Header("Oscillation Settings")]
    public Vector3 rotationAxis = Vector3.forward;
    public float range = 45f;
    public float frequency = 1f;

   

    [Header("Accuracy Increase")]
    [Tooltip("Amount of accuracy gained per second while player is detected.")]
    public float accuracyPerSecond = 0.5f;

    [Header("Deteccion")]
  
    public float deteccionMeter;
    public float maxDeteccion = 10;
    public Vector2 playerLocation;

    [SerializeField] private bool alert= false;
    private bool playerInside;
    [SerializeField] private float deteccionSpeed;

    private Quaternion initialRotationOffset;
    [SerializeField] private float offset;
    [SerializeField]private float  cameraSpeed;
    void Start()
    {
        target = GameObject.FindWithTag("Target");
          death   = target.GetComponent<death>();
        deteccionSystem = GetComponentInChildren<DeteccionSystem>();
       player = GameObject.FindWithTag("Player");

        deteccionMeter = 0;
        alert = false;

        if (pivotTransform == null)
        {
            pivotTransform = transform.parent;
        }

        DetectionManager.Instance.OnGlobalAlert += ActivateGlobalAlert;
        
        DetectionManager.Instance.OnGlobalAlertReset += HandleGlobalReset;



        if (pivotTransform == null)
        {
            Debug.LogError("OscillatorCamera must be attached to a child object with a static parent pivot.");
            enabled = false;
            return;
        }
        initialRotationOffset = pivotTransform.localRotation;

    }

    void Update()
    {
        Debug.Log("target: " + target);
        Debug.Log("player: " + player);
        Debug.Log("deteccionSystem: " + deteccionSystem);
        Debug.Log("pivot: " + pivotTransform);

        deteccionSystem.Deteccion(maxDeteccion, deteccionMeter);

        playerLocation = new Vector2(player.transform.position.x, player.transform.position.y);

        if (!alert)
        {
            float rotationAngle = Mathf.Sin(Time.time * frequency) * range;

            Quaternion oscillationRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis);

            pivotTransform.localRotation = initialRotationOffset * oscillationRotation;
        }

        if (deteccionMeter >= maxDeteccion)
        {
            alert = true;
            deteccionMeter = maxDeteccion;
            DetectionManager.Instance.TriggerGlobalAlert();
        }

        if (playerInside)
        {
            deteccionMeter = Mathf.MoveTowards(deteccionMeter, maxDeteccion, deteccionSpeed * Time.deltaTime);
        }
        else
        {
            if (alert == false)
            {
                deteccionMeter = Mathf.MoveTowards(deteccionMeter, 0, (deteccionSpeed / 2) * Time.deltaTime);
            }
        }


        if(alert)
        {
            //comportamient en alerta
            death.spawn = true;
            Vector3 dir = player.transform.position - pivotTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f + offset;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, targetRotation, cameraSpeed * Time.deltaTime);
        }

        if(death.resetPlayer)
        {
            death.spawn = false;
            deteccionMeter = 0;
            alert = false;
            death.resetPlayer = false;
           
        }
        
    }
    public void ActivateGlobalAlert()
    {
        deteccionMeter = maxDeteccion;
        alert = true;
        deteccionSystem.Deteccion(maxDeteccion, deteccionMeter);
    }
    private void OnReset()
    {
        if (DetectionManager.Instance != null)
            DetectionManager.Instance.OnGlobalAlertReset -= HandleGlobalReset;
    }
    private void HandleGlobalReset()
    {
        
        deteccionMeter = 0f;
        alert = false;
        playerInside = false;
      
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            playerInside = true;  
            

            Debug.Log("hola");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            playerInside = false;

        }
    }
   
}
