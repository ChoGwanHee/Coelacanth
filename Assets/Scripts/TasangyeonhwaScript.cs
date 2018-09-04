using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasangyeonhwaScript : MonoBehaviour {

    public GameObject fireEfx;

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
        animator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(5.0f);
        animator.SetTrigger("Fire");
        yield return new WaitForSeconds(5.0f);
        animator.SetBool("IsOpen", false);

        started = false;
    }

    public void Effect()
    {
        Instantiate(fireEfx, firePos.position, Quaternion.identity);
    }
}
