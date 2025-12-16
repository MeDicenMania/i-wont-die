using UnityEngine;
using TMPro;

public class TriggerText2D : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    private void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (messageText != null)
                messageText.gameObject.SetActive(true);

            Debug.Log("Player entered trigger.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (messageText != null)
                messageText.gameObject.SetActive(false);

            Debug.Log("Player exited trigger.");
        }
    }
}
