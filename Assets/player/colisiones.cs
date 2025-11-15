using UnityEngine;
using UnityEngine.UIElements;

public class colisiones : MonoBehaviour
{
    public bool enPared;
    public bool onPole;
    public bool onPoleH;
    public bool enPiso;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    public LayerMask ground;
    public LayerMask pole;
    public LayerMask poleh; // pole pero horizontal
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Vector2 climbOffset;
   

    private Color debugCollisionColor = Color.red;
    public Vector2 tamaño;
    public Vector2 climb;
    [HideInInspector] public Vector3 poleCenter;

    public Vector2 tamañoabj;
    void Start()
    {
        
    }

 
    void Update()
    {
        enPiso =  Physics2D.OverlapBox((Vector2)transform.position + bottomOffset,tamañoabj,0,ground);
   
        enPared = Physics2D.OverlapBox((Vector2)transform.position + rightOffset,tamaño,0,ground) ||
                 Physics2D.OverlapBox((Vector2)transform.position + leftOffset,tamaño,0,ground);

        onRightWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, tamaño,0, ground);
        onLeftWall = Physics2D.OverlapBox((Vector2)transform.position + leftOffset, tamaño,0, ground);

        wallSide = onRightWall ? -1 : 1;
        Collider2D poleHit = Physics2D.OverlapBox((Vector2)transform.position + climbOffset, climb, 0, pole);
        Collider2D poleHor = Physics2D.OverlapBox((Vector2)transform.position + climbOffset, climb, 0, poleh);

        onPole = poleHit != null;
        onPoleH = poleHor != null;
        if (onPoleH)
        {
            poleCenter = poleHor.bounds.center;
        }

        if (onPole)
        {
            poleCenter = poleHit.bounds.center;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset,};
        
        Gizmos.DrawWireCube((Vector2)transform.position + climbOffset, climb);
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, tamañoabj);
        Gizmos.DrawWireCube((Vector2)transform.position + rightOffset, tamaño);
        Gizmos.DrawWireCube((Vector2)transform.position + leftOffset, tamaño);
    }
}



