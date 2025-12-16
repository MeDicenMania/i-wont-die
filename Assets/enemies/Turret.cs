using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
    [Header("References")]
  
    private GameObject target;
 [SerializeField]   private GameObject player;
    private death death;
    [SerializeField] private Transform pivotTransform;
    [Header("Oscillation Settings")]
    public Vector3 rotationAxis = Vector3.forward;
    public float range = 45f;
    public float frequency = 1f;

    public GameObject arma;

    [Header("Accuracy Increase")]
    [Tooltip("Amount of accuracy gained per second while player is detected.")]
    public float accuracyPerSecond = 0.5f;

    [Header("Deteccion")]
    [SerializeField] private float initialZRotation;
    public float deteccionMeter;
    public float maxDeteccion = 10;
    public Vector2 playerLocation;

    [SerializeField] private bool alert = false;
    private bool playerInside;
    [SerializeField] private float deteccionSpeed;

    private Quaternion initialRotationOffset;
    [SerializeField] private float offset;
    [SerializeField] private float cameraSpeed;
    private Vector3 dir;
    [SerializeField] private GameObject bala;
    private float timer;
    [SerializeField] private float cadencia;
    [SerializeField] private float miPene;
    private bool inVision;
    public bool isFlip;
    public bool moves;
    public float velocidadx;
    [SerializeField] private float fausto;
    private float originalX;
    [SerializeField] private float velocidadX;
    [SerializeField] private float offsetmov;
    [SerializeField] private float limitex;
    private float originalValue;
    void Start()
    {
       
        target = GameObject.FindWithTag("Target");
        death = target.GetComponent<death>();
       
        player = GameObject.FindWithTag("Player");

        deteccionMeter = 0;
        alert = false;
        pivotTransform = transform.parent;
        DetectionManager.Instance.OnGlobalAlert += ActivateGlobalAlert;

        DetectionManager.Instance.OnGlobalAlertReset += HandleGlobalReset;


        if (pivotTransform == null)
        {
            pivotTransform = transform.parent;
        }

        if (pivotTransform == null)
        {
            Debug.LogError("No se asignó el pivot en la torreta.", this);
            enabled = false;
            return;
        }
        if (isFlip)
        {
            initialZRotation = 180f; 

        }
        else
        {
            initialZRotation = 0f; 
        }
        pivotTransform.rotation = Quaternion.Euler(0, 0, initialZRotation);

          originalX = pivotTransform.transform.position.x;
        originalValue = velocidadX;
    }

    void Update()
    {
        if (moves)
        {
            if (velocidadX > 0 && pivotTransform.position.x > limitex)
            {
                velocidadX *= -1f;
            }

            if (velocidadX < 0 && pivotTransform.position.x < limitex + offsetmov)
            {
                velocidadX *= -1f;
            }
            pivotTransform.position += new Vector3(velocidadx * velocidadX * Time.deltaTime, 0, 0);


        }

        


        playerLocation = new Vector2(player.transform.position.x, player.transform.position.y);

       

        if (!alert)
        {
            float relativeOscillationAngle = Mathf.Sin(Time.time * frequency) * range;

            float targetGlobalAngle = initialZRotation + relativeOscillationAngle;

            pivotTransform.rotation = Quaternion.Euler(0, 0, (targetGlobalAngle + fausto)* miPene );
            
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


        if (alert)
        {
            if (playerInside)
            {


              timer += Time.deltaTime;
              if( timer >= cadencia)
              {
                    Quaternion bulletRotation = arma.transform.rotation;
                    if (isFlip)
                    {
                        bulletRotation *= Quaternion.Euler(0f, 0f, 180f);
                    }

                    Instantiate(bala, arma.transform.position, bulletRotation);
                    timer = 0;
               }
            }

            death.spawn = true;
            dir = player.transform.position - pivotTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f + offset;

            float relativeAngle = Mathf.DeltaAngle(initialZRotation, angle);
            float clampedAngle = initialZRotation + relativeAngle;
            if (isFlip)
            {
                relativeAngle = Mathf.Clamp(relativeAngle, -range, range);  

            }
            else
            {
                relativeAngle = Mathf.Clamp(relativeAngle, -range, range);
            }

            
          Quaternion targetRotation = Quaternion.Euler(0f, 0f, clampedAngle);

           pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, targetRotation, cameraSpeed * Time.deltaTime);
        }

        if (death.resetPlayer)
        {
            death.spawn = false;
            deteccionMeter = 0;
            alert = false;
            death.resetPlayer = false;
            GameObject[] balas = GameObject.FindGameObjectsWithTag("Bala");

            foreach (GameObject b in balas)
            {
                Destroy(b);
            }
            if (moves)
            {
                pivotTransform.transform.position = new Vector2(originalX, pivotTransform.transform.position.y);

            }

        }

    }
    public void ActivateGlobalAlert()
    {
        deteccionMeter = maxDeteccion;
        alert = true;
       
    }
    private void OnReset()
    {
        if (DetectionManager.Instance != null)
            DetectionManager.Instance.OnGlobalAlertReset -= HandleGlobalReset;
      

    }
    private void HandleGlobalReset()
    {
        if(moves)
        {
            pivotTransform.transform.position = new Vector2(originalX, pivotTransform.transform.position.y);
            
            if(velocidadX < 0)
            {
                velocidadX *= -1;
            }
        }
       
        deteccionMeter = 0f;
        alert = false;
        playerInside = false;

        GameObject[] balas = GameObject.FindGameObjectsWithTag("Bala");

        foreach (GameObject b in balas)
        {
            Destroy(b);
        }
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


