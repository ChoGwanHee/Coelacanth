﻿using UnityEngine;

/// <summary>
/// 로망 폭죽 투사체 클래스
/// </summary>
public class ProjectileRoman : BaseProjectile
{
    protected override void Update()
    {
        if (!photonView.isMine) return;

        // 수명이 다 되면 삭제
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;
        if (other.CompareTag("Item")) return;

        Explosion();
    }

    /// <summary>
    /// 폭발하여 일정 반경 안의 물체를 밀어내고 피해를 줍니다.
    /// </summary>
    private void Explosion()
    {
        GetComponent<Collider>().enabled = false;

        Vector3 fixedPosition = transform.position - (rb.velocity.normalized * speed * Time.fixedDeltaTime);
        Collider[] effectedObjects = Physics.OverlapSphere(fixedPosition, hitRadius, dynamicObjMask);

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            Vector3 direction = Vector3.Scale(effectedObjects[i].transform.position - fixedPosition, new Vector3(1, 0, 1)).normalized;
            PhotonView objPhotonView = effectedObjects[i].GetComponent<PhotonView>();
            

            if (effectedObjects[i].CompareTag("Player"))
            {
                PlayerStat effectedPlayer = effectedObjects[i].GetComponent<PlayerStat>();

                if (!effectedPlayer.PC.isUnbeatable)
                {
                    objPhotonView.RPC("Pushed", objPhotonView.owner, (direction * hitForce));
                    objPhotonView.RPC("DamageShake", objPhotonView.owner, damage, 1, photonView.ownerId);

                    if (objPhotonView.ownerId == photonView.ownerId)
                    {
                        // 본인 피격시 점수 처리
                        effectedPlayer.AddScore(-20);
                    }
                    else
                    {
                        // 다른 사람 피격시 점수 처리
                        effectedPlayer.PC.Pushed(direction * hitForce * 0.5f);
                        effectedPlayer.AddScore(-10);
                        GameManagerPhoton._instance.GetPlayerByOwnerId(photonView.ownerId).AddScore(gainScore);
                    }
                }

                // 피격 이펙트
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(fixedPosition);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
            else
            {
                objPhotonView.RPC("Pushed", PhotonTargets.MasterClient, (direction * hitForce));
            }
        }

        photonView.RPC("PlayEndSound", PhotonTargets.All, null);

        PhotonNetwork.Instantiate("Prefabs/Romang_Hit_fx", fixedPosition, transform.rotation, 0);
        PhotonNetwork.Destroy(gameObject);
    }
}
