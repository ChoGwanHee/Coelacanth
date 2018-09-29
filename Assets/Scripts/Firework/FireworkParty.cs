using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 파티 폭죽 클래스
/// </summary>
[CreateAssetMenu(menuName = "Firework/Party")]
public class FireworkParty : Firework {

    /// <summary>
    /// 공격 범위의 각도 (호의 중심각)
    /// </summary>
    public float hitAngle;

    /// <summary>
    /// 몇 단계로 나눠 계산하는지
    /// </summary>
    public int step;

    public override void Execute(FireworkExecuter executer)
    {
        // 자기자신이 공격에 맞지 않게 콜라이더 잠시 꺼줌
        CapsuleCollider self = executer.GetComponent<CapsuleCollider>();
        self.enabled = false;

        Vector3 firePos = executer.transform.position + executer.transform.forward * self.radius +
            Vector3.up * (executer.firePoint.position.y - executer.transform.position.y);

        List<Collider> effectedObjects = new List<Collider>();
        float stepAngleSize = hitAngle / step;
        float curAngle;
        int curStep = step / 2;
        RaycastHit[] hits;


        for (int i = 0; i <= step; i++)
        {
            curAngle = executer.transform.eulerAngles.y - hitAngle / 2 + stepAngleSize * curStep;

            if(curStep <= 0)
            {
                curStep = step / 2 + 1;
            }
            else if(curStep <= step / 2)
            {
                curStep--;
            }
            else
            {
                curStep++;
            }

            hits = Physics.RaycastAll(firePos, DirFromAngle(curAngle), hitRadius, LayerMask.GetMask("DynamicObject"), QueryTriggerInteraction.Ignore);

            for (int j = 0; j < hits.Length; j++)
            {
                PhotonView objPhotonView = hits[j].collider.GetComponent<PhotonView>();
                objPhotonView.RPC("Pushed", PhotonTargets.All, (DirFromAngle(curAngle) * hitForce));

                if (hits[j].collider.CompareTag("Player"))
                {
                    hits[j].collider.GetComponent<PlayerController>().Pushed(DirFromAngle(curAngle) * hitForce * 0.5f);
                    objPhotonView.RPC("DamageShake", objPhotonView.owner, 5, executer.photonView.ownerId);
                    Vector3 efxPos = hits[j].collider.GetComponent<CapsuleCollider>().ClosestPointOnBounds(executer.firePoint.position);
                    PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);

                    // 점수 처리
                    hits[j].collider.GetComponent<PlayerStat>().AddScore(-10);
                    executer.Stat.AddScore(gainScore);
                }

                effectedObjects.Add(hits[j].collider);
                hits[j].collider.enabled = false;
            }

            if (hits.Length > 0)
                Debug.DrawLine(firePos, firePos + DirFromAngle(curAngle) * hitRadius, Color.red, 3.0f);
            else
                Debug.DrawLine(firePos, firePos + DirFromAngle(curAngle) * hitRadius, Color.green, 3.0f);
        }


        for(int i=0; i<effectedObjects.Count; i++)
        {
            effectedObjects[i].enabled = true;
        }

        PhotonNetwork.Instantiate("Prefabs/party_firework_boom_fx", firePos, executer.transform.rotation, 0);

        self.enabled = true;

        executer.DecreaseAmmo();
    }

    private Vector3 DirFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
