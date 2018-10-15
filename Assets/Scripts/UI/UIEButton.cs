using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIEButton : MonoBehaviour {

    [Serializable]
    public struct ButtonData
    {
        public bool useAnimator;
        public Sprite sprite;
        public bool isWorldCoord;
        public Vector3 offset;
    }

    public ButtonData[] buttonData;
    public Transform target;
    
    private bool isWorldCoord;
    public Vector3 offset = Vector3.zero;
    private Animator animator;
    private Image image;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    public void SetActivate(bool active, Transform newTarget = null, int buttonType = 0)
    {
        if (active)
        {
            target = newTarget;
            animator.enabled = buttonData[buttonType].useAnimator;
            image.sprite = buttonData[buttonType].sprite;
            image.SetNativeSize();
            isWorldCoord = buttonData[buttonType].isWorldCoord;
            offset = buttonData[buttonType].offset;
            StartCoroutine(FollowTarget());
        }
        else
        {
            target = null;
            transform.position = new Vector3(-100, 0);
        }
    }

    private IEnumerator FollowTarget()
    {
        while (true)
        {
            if (target == null)
            {
                break;
            }
            else
            {
                if(isWorldCoord)
                    transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
                else
                    transform.position = Camera.main.WorldToScreenPoint(target.position) + offset;
            }
            yield return null;
        }
        
    }
}
