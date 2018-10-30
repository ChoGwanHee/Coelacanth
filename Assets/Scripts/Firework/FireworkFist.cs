using UnityEngine;

/// <summary>
/// 주먹 공격 클래스
/// </summary>
[CreateAssetMenu(menuName = "Firework/Fist")]
public class FireworkFist : Firework
{

    public override void Execute(FireworkExecuter executer)
    {
        // 자기자신이 공격에 맞지 않게 콜라이더 잠시 꺼줌
        Collider self = executer.GetComponent<Collider>();
        self.enabled = false;

        Collider[] effectedObjects = Physics.OverlapSphere(executer.firePoint.position, hitRadius, LayerMask.GetMask("DynamicObject"), QueryTriggerInteraction.Ignore);
        Vector3 direction = executer.transform.forward;
        Vector3 finalForce = direction * hitForce * executer.forceFactor;

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            PhotonView objPhotonView = effectedObjects[i].GetComponent<PhotonView>();

            if (effectedObjects[i].CompareTag("Player"))
            {
                PlayerStat effectedPlayer = effectedObjects[i].GetComponent<PlayerStat>();

                if(!effectedPlayer.PC.isUnbeatable)
                {
                    // 넉백
                    effectedPlayer.PC.Pushed(finalForce * 0.5f);
                    objPhotonView.RPC("Pushed", objPhotonView.owner, finalForce);
                    objPhotonView.RPC("Damage", objPhotonView.owner, Mathf.RoundToInt(damage * executer.damageFactor), executer.photonView.ownerId);

                    // 점수 처리
                    effectedPlayer.AddScore(-10);
                    executer.Stat.AddScore(gainScore);
                }
                
                // 피격 이펙트
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(executer.firePoint.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
            else
            {
                objPhotonView.RPC("Pushed", PhotonTargets.All, finalForce);
            }
        }

        self.enabled = true;
    }
}
