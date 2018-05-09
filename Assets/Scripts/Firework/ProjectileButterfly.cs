using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileButterfly : BaseProjectile
{
    public Transform target;
    public float rotateAngle = 30f;
    protected override void Update()
    {
        if (!photonView.isMine) return;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Debug.Log("끝");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;

        if (other.CompareTag("Item")) return;

    }

    private void Rotate()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        //Quaternion targetRotation = Quaternion.
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateAngle * Time.deltaTime);
    }
}
