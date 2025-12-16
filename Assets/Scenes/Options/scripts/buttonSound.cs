using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
            MusicManager.Instance.PlaySFX(hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
            MusicManager.Instance.PlaySFX(clickSound);
    }
}