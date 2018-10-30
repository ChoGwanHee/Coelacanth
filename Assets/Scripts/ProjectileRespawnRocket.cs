using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRespawnRocket : BaseProjectile
{
    public GameObject hitEfx_ref;

    private void Start()
    {
        SetSpeed(speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if (PhotonNetwork.isMasterClient)
            {
                Explosion();
            }
            Instantiate(hitEfx_ref, transform.position, Quaternion.identity);
            FMODUnity.RuntimeManager.PlayOneShot(endSound);
            Destroy(gameObject);
        }
    }

    public override void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        rb.velocity = -transform.up * speed;
    }

    /// <summary>
    /// 폭발하여 일정 반경 안의 물체를 밀어내고 피해를 줍니다.
    /// </summary>
    private void Explosion()
    {
        GetComponent<Collider>().enabled = false;

        Collider[] effectedObjects = Physics.OverlapSphere(transform.position, hitRadius, dynamicObjMask);

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            Vector3 direction = Vector3.Scale(effectedObjects[i].transform.position - transform.position, new Vector3(1, 0, 1)).normalized;
            PhotonView objPhotonView = effectedObjects[i].GetComponent<PhotonView>();


            if (effectedObjects[i].CompareTag("Player"))
            {
                PlayerStat effectedPlayer = effectedObjects[i].GetComponent<PlayerStat>();

                if (!effectedPlayer.PC.isUnbeatable)
                {
                    objPhotonView.RPC("Pushed", objPhotonView.owner, (direction * hitForce));
                    objPhotonView.RPC("DamageShake", objPhotonView.owner, damage, 3, -1);
                }

                // 피격 이펙트
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
            else
            {
                objPhotonView.RPC("Pushed", PhotonTargets.All, (direction * hitForce));
            }
        }
    }
}
