using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;

    private void Start()
    {
        m_Camera = Camera.main;
    }

    void FixedUpdate()
    {
        //transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
        //m_Camera.transform.rotation * Vector3.forward);
        //Vector3 direction = (transform.position -m_Camera.transform.position).normalized;

        //transform.LookAt(direction);

        transform.LookAt(m_Camera.transform.position);

        
    }
}