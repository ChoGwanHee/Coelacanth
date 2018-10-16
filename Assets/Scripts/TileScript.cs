using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private Rigidbody rb;
    private Animator anim;


	void Awake () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }
	
	public void SetShake()
    {
        anim.SetTrigger("Shake");
    }
}
