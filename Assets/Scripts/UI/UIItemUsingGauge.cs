using UnityEngine;
using UnityEngine.UI;

public class UIItemUsingGauge : MonoBehaviour {

    public Image gauge;
    public Vector3 offset = new Vector3(0, 2, 0);
    private Transform target;

    private void FixedUpdate()
    {
        if(target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if(newTarget == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gauge.fillAmount = 0.0f;
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
            gameObject.SetActive(true);
        }
    }

    public void SetAmount(float val)
    {
        gauge.fillAmount = val;
    }
}

