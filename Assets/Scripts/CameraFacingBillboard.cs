using UnityEngine;
using System.Collections;

/// <summary>
/// 카메라를 항상 정면으로 바라보게 해주는 스크립트입니다.
/// </summary>
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