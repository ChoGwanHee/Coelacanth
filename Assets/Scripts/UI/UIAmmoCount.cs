using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoCount : MonoBehaviour {

    public Vector3 offset = new Vector3(20, -20, 0);

    public Text amountText;

    private void Awake()
    {
        amountText = GetComponent<Text>();
    }

    private void Update()
    {
        Vector3 screenPos = Input.mousePosition + offset;

        transform.position = screenPos;
    }

    public void SetAmount(int amount)
    {
        if (amountText == null) return;

        if(amount < 0)
        {
            amountText.enabled = false;
        }
        else
        {
            amountText.enabled = true;
            amountText.text = (amount < 10 ? "0" : "") + amount.ToString();
        }
        
    }
}
