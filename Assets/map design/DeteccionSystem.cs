using UnityEngine;
using UnityEngine.UI;
public class DeteccionSystem : MonoBehaviour
{
    [Header("Referencias")]
    private Image _image;
    Camera _camera;
    public Sprite exclamacion;
    public Sprite interrogacion;
    void Start()
    {
        _camera = GameObject.FindWithTag("camara").GetComponent<Camera>();
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Deteccion(float maxDeteccion,float deteccionMeter)
    {
        
        if (deteccionMeter >= maxDeteccion)
        {
            _image.sprite = exclamacion;
            _image.fillAmount = 1f;
        }
        else if ( deteccionMeter < maxDeteccion )   
        {
           
            _image.sprite = interrogacion;
            _image.fillAmount = deteccionMeter / maxDeteccion;
        }
    }


}
