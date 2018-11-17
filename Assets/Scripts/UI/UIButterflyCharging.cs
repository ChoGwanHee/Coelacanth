using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButterflyCharging : MonoBehaviour {

    public Image countImg;
    public RectTransform outerCircle;

    public Transform target;
    public Vector3 offset = new Vector3(0, 2, 0);

    public Sprite[] countImg_ref;


    private void Update()
    {
        if(target !=null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);

            transform.position = screenPos;
        }
    }


    public void SetRatio(float ratio)
    {
        float angle = -360f * ratio;
        Quaternion targetRotation = Quaternion.identity;

        targetRotation.eulerAngles = new Vector3(0, 0, angle);

        outerCircle.rotation = targetRotation;
    }

    public void SetAmount(int amount)
    {
        countImg.sprite = countImg_ref[amount];
    }

    public void SetActivate(bool active)
    {
        if (active)
        {
            if (target != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);

                transform.position = screenPos;
            }
        }
        

        gameObject.SetActive(active);
        Debug.Log(gameObject + ", " + active);
    }
}
