using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 테이블 위에 올라가 있는 물체
/// </summary>
public class TableTopObject : MonoBehaviour {

    /// <summary>
    /// 오브젝트가 사라지는 시간
    /// </summary>
    public float disappearTime = 3.0f;

    /// <summary>
    /// 리스폰 지연 시간
    /// </summary>
    public float respawnDelay = 0.4f;

    /// <summary>
    /// 오브젝트의 원래 위치
    /// </summary>
    private Vector3 originPos;

    /// <summary>
    /// 오브젝트의 원래 회전값
    /// </summary>
    private Quaternion originRot;

    private bool alive = true;

    private Rigidbody rb;
    private Collider col;
    private MeshRenderer mr;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();

        originPos = transform.position;
        originPos.y += 3.1f;
        originRot = transform.rotation;
    }

    private void Update()
    {
        if (alive)
        {
            if(transform.position.y < 0)
            {
                StartCoroutine(Hide());
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (alive)
        {
            if(collision.gameObject.CompareTag("Ground"))
            {
                if(rb.IsSleeping())
                {
                    StartCoroutine(Hide());
                }
            }
        }
    }

    public void Respawn()
    {
        StopAllCoroutines();
        StartCoroutine(WaitRespawn());
    }

    private IEnumerator WaitRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        mr.enabled = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = originPos;
        transform.rotation = originRot;

        col.enabled = true;
        rb.useGravity = true;
        alive = true;
    }

    /// <summary>
    /// 오브젝트가 서서히 사라지게 합니다.
    /// </summary>
    private IEnumerator Disappear()
    {
        Color color = mr.material.color;
        float elapsedTime = 0;
        float ratio = 1;

        alive = false;
        col.enabled = false;
        rb.useGravity = false;

        while(ratio > 0)
        {
            ratio = 1 - (elapsedTime / disappearTime);

            color.a = ratio;
            mr.material.color = color;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        color.a = 0f;
        mr.material.color = color;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
    }

    private IEnumerator Hide()
    {
        alive = false;
        col.enabled = false;
        rb.useGravity = false;

        yield return new WaitForSeconds(disappearTime);

        mr.enabled = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
    }

}
