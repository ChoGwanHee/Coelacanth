using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTasanyeonhwaUp : MonoBehaviour {

    public Vector3 startVelocity;
    public float lifeTime = 7.0f;
    private float elapsedTime = 0;

    private Rigidbody rb;


	void Awake () {
        rb = GetComponent<Rigidbody>();
	}

    private void Start()
    {
        rb.velocity = startVelocity;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifeTime)
            Destroy(gameObject);
    }
}
