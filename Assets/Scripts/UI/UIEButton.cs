using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEButton : MonoBehaviour {

    public Transform target;
    public Vector3 offset = new Vector3(0, 2, 0);


    public void SetActivate(bool active, Transform newTarget = null)
    {
        if (active)
        {
            target = newTarget;
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
                transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
            }
            yield return null;
        }
        
    }
}
