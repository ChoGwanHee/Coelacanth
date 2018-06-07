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

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            effectedObjects[i].gameObject.GetPhotonView().RPC("Pushed", PhotonTargets.All, (direction * hitForce));

            if (effectedObjects[i].CompareTag("Player"))
            {
                effectedObjects[i].gameObject.GetPhotonView().RPC("Damage", PhotonTargets.All, damage, executer.photonView.ownerId);
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(executer.firePoint.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);

                // 점수 처리
                PhotonNetwork.player.AddScore(gainScore);
            }
        }

        self.enabled = true;
    }
}
