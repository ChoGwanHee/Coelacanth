using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [FMODUnity.EventRef]
    public string mouseDownSound;

    [FMODUnity.EventRef]
    public string mouseOverSound;


    public void OnPointerDown(PointerEventData eventData)
    {
        FMODUnity.RuntimeManager.PlayOneShot(mouseDownSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FMODUnity.RuntimeManager.PlayOneShot(mouseOverSound);
    }
}
