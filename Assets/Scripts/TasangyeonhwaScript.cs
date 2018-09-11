using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasangyeonhwaScript : MonoBehaviour {

    /// <summary>
    /// 떨어지는 폭죽 개수
    /// </summary>
    public int fallingAmount;
    /// <summary>
    /// 다음 폭죽 대기시간
    /// </summary>
    public float fallingInterval;

    /// <summary>
    /// 재생성 대기 시간
    /// </summary>
    public float respawnTime;

    /// <summary>
    /// 위로 올라가는 타상연화 폭죽
    /// </summary>
    public GameObject tasangUp;
    /// <summary>
    /// 떨어지는 타상연화 폭죽
    /// </summary>
    public GameObject tasangProjectile;

    /// <summary>
    /// 위로 올라갈 때 발사되는 위치
    /// </summary>
    public Transform firePos;


    Animator animator;

    bool started = false;

	void Start () {
        animator = GetComponent<Animator>();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !started)
        {
            StartCoroutine(TestProcess());
        }
    }

    private IEnumerator TestProcess()
    {
        started = true;
        SetState(0);
        yield return new WaitForSeconds(5.0f);
        SetState(1);
        yield return new WaitForSeconds(5.0f);
        SetState(2);

        started = false;
    }

    public void SetState(int stateNum)
    {
        switch (stateNum)
        {
            case 0:
                animator.SetBool("IsOpen", true);
                break;
            case 1:
                animator.SetTrigger("Fire");
                break;
            case 2:
                animator.SetBool("IsOpen", false);
                break;
        }
    }

    public void FireEffect()
    {
        Instantiate(tasangUp, firePos.position, Quaternion.identity);
    }

    public void Bombing()
    {
        
    }

    private IEnumerator BombingProcess()
    {

        yield return null;
    }
}
