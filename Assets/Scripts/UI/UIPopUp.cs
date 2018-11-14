using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUp : MonoBehaviour {

    public Text content;

    private Button button;
    public Button Button { get { return button; } }


    private void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(Exit);
    }

    public void ActiveButton(bool active)
    {
        button.gameObject.SetActive(active);
    }

    public void PopUp(string contentText)
    {
        gameObject.SetActive(true);
        content.text = contentText;
    }

    public void SetSize(float width, float height)
    {
        RectTransform tr = transform as RectTransform;

        tr.sizeDelta = new Vector2(width, height);
    }

    private void Exit()
    {
        gameObject.SetActive(false);
    }
}
