using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private Rigidbody rb;
    private Collider col;
    

	void Awake () {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
    public void Fall()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        col.enabled = false;
    }

    private void TileDisable()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
