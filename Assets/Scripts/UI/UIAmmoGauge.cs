using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoGauge : MonoBehaviour {

    public Image gaugeImg;

    public Vector3 offset = new Vector3(20, 20, 0);

    private void Update()
    {
        Vector3 screenPos = Input.mousePosition + offset;

        transform.position = screenPos;
    }

    public void SetRatio(float ratio)
    {
        gaugeImg.fillAmount = ratio;
    }
}
