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

    /// <summary>
    /// 줄의 넓이
    /// </summary>
    private float lineWidth = 0.01f;
    

    private Rigidbody rb;
    private Transform objectedTransform;

    private LineRenderer lineRenderer;

	void Start () {
        rb = GetComponent<Rigidbody>();

        ConfigurableJoint cj = GetComponent<ConfigurableJoint>();
        objectedTransform = cj.connectedBody.transform;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.SetPosition(0, objectedTransform.position);
        lineRenderer.SetPosition(1, transform.position);
	}

    private void FixedUpdate()
    {
        if(isGraviter)
            rb.AddForce(individualGravity, ForceMode.Acceleration);

        if(lineRenderer != null)
        {
            lineRenderer.SetPosition(0, objectedTransform.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
