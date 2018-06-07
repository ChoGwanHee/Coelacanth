using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonScript : MonoBehaviour {

    /// <summary>
    /// 중력이 적용되고 있는지 여부
    /// </summary>
    public bool isGraviter;

    /// <summary>
    /// 항상 적용되는 힘
    /// </summary>
    public Vector3 individualGravity;
    

    private Rigidbody rb;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void FixedUpdate()
    {
        if(isGraviter)
            rb.AddForce(individualGravity, ForceMode.Acceleration);
    }
}
