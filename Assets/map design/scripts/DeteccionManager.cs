using System;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    [SerializeField] private GameObject bala;
    public static DetectionManager Instance;
    private GameObject death;
    private death target;
    public event Action OnGlobalAlert;
    public event Action OnGlobalAlertReset;

    public bool globalAlertActive = false;

    private void Awake()
    {
        
        Instance = this;
    }

    public void TriggerGlobalAlert()
    {
        if (globalAlertActive) return; // evitar spam

        globalAlertActive = true;

        Debug.Log("GLOBAL ALERT ACTIVADO!");
        OnGlobalAlert?.Invoke();
    }

    public void ResetGlobalAlert()
    {
        globalAlertActive = false;
        OnGlobalAlertReset?.Invoke();
       
    }
}