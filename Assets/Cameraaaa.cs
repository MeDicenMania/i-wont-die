
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Cameraaaa : MonoBehaviour
{
    
    public GameObject mira;
    public  death death;
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            death.spawn = true;
        }
    }

}



