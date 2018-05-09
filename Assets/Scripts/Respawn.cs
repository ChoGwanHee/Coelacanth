using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    private Vector3 originPos;
    private Quaternion originQuaternion;
    private Rigidbody rb;

	void Start () {
        originPos = transform.position;
        originPos.y += 3.0f;
        originQuaternion = transform.rotation;
        rb = GetComponent<Rigidbody>();
        
    }
	
	void Update () {
		if(transform.position.y < -15)
        {
            transform.position = originPos;
            transform.rotation = originQuaternion;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
	}
}
