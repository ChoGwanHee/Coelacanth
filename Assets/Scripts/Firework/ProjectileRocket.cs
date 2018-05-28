using UnityEngine;

/// <summary>
/// 로켓 폭죽 투사체 클래스
/// </summary>
public class ProjectileRocket : BaseProjectile
{
    protected override void Update()
    {
        if (!photonView.isMine) return;

        // 수명이 다 되면 투사체 제거
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
        Collider[] effectedObjects = Physics.OverlapSphere(transform.position, hitRadius, dynamicObjMask);

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            Vector3 direction = Vector3.Scale(effectedObjects[i].transform.position - transform.position, new Vector3(1, 0, 1)).normalized;

            effectedObjects[i].gameObject.GetPhotonView().RPC("Pushed", PhotonTargets.All, (direction * hitForce));

            if (effectedObjects[i].CompareTag("Player"))
            {
                effectedObjects[i].gameObject.GetPhotonView().RPC("Damage", PhotonTargets.All, damage);
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
        }

        photonView.RPC("PlayEndSound", PhotonTargets.All, null);

        PhotonNetwork.Instantiate("Prefabs/Rocket_boom_Hit_fx", transform.position, transform.rotation, 0);
        PhotonNetwork.Destroy(gameObject);
    }
}
