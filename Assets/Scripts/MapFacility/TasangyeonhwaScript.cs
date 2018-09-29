using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasangyeonhwaScript : MonoBehaviour {

    public enum TasangState
    {
        Camo,
        Appear,
        Fire
    }

    /// <summary>
    /// 위로 올라가는 타상연화 폭죽
    /// </summary>
    public GameObject tasangUp;
    
    /// <summary>
    /// 위로 올라갈 때 발사되는 위치
    /// </summary>
    public Transform firePos;

    
    Animator animator;


	void Start () {
        animator = GetComponent<Animator>();
	}

    public void SetState(TasangState stateNum)
    {
        switch (stateNum)
        {
            case TasangState.Camo:
                animator.SetBool("IsOpen", false);
                break;
            case TasangState.Appear:
                animator.SetBool("IsOpen", true);
                break;
            case TasangState.Fire:
                animator.SetTrigger("Fire");
                break;
        }
    }

    public void FireEffect()
    {
        Instantiate(tasangUp, firePos.position, Quaternion.identity);
    }
    

    
}
