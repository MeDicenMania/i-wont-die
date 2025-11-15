using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.PlayerSettings;

public class camera : MonoBehaviour
{
    [SerializeField] private Vector2 rangeleft;
    [SerializeField] private Vector2 rangeright;
   [SerializeField] private Vector2 camDir;
    [SerializeField] private float camvel;
    private float timeStart = 5;
   [SerializeField]  private float time;
    [SerializeField] private float transformx;
    private float direccion = 1;
   public  GameObject camerasize;
    public float distancia;
    public float distancia2;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       

        transformx = camerasize.transform.position.x;
        time += Time.deltaTime;

        if (time > timeStart)  
        {
            CamMov();
        }
    }
    private void CamMov()
    {
       transform.Rotate(0,0,direccion * camvel );

        //Vector2 leftDir = (rangeleft - (Vector2)transform.position).normalized;
        //Vector2 rightDir = (rangeright - (Vector2)transform.position).normalized;

        camDir = (Vector2)transform.position + (Vector2)(transform.up) * 3;


        //float angleToLeft = Vector2.SignedAngle(leftDir, camDir);
        //float angleToRight = Vector2.SignedAngle(camDir, rightDir);


        distancia = Vector2.Distance(camDir, rangeleft);
        distancia2 = Vector2.Distance(camDir, rangeright);


        //if (angleToLeft < 0)
        //{
        //    direccion = 1;
        //}
       
        //else if (angleToRight < 0)
        //{
        //    direccion = -1;
        //}
        if( distancia >= 1)
        {
            Vector2.MoveTowards(camDir, rangeright, 3);
            direccion = 1;
        }
        if (distancia2 >= 1)
        {
            direccion = -1;
        }

    }

    private void OnDrawGizmos()
    {
       Gizmos.color = Color.yellow;
        Gizmos.DrawLine((Vector2)transform.position, camDir);
        Gizmos.DrawLine((Vector2)transform.position , rangeleft);
        Gizmos.DrawLine((Vector2) transform.position, rangeright);
        
    }
}
