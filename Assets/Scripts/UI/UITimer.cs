using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour {

    public bool timerEnable = false;

    private Text displayText;

    void Start()
    {
        displayText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        SetTime(GameManagerPhoton._instance.RemainGameTime);
    }

    public void SetTime(float num)
    {
        if (num < 0)
        {
            displayText.text = "0";
        }
        else
        {
            displayText.text = Mathf.FloorToInt(num).ToString();
        }
    }
}
