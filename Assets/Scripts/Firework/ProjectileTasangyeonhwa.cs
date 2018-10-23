using UnityEngine;

public class ProjectileTasangyeonhwa : BaseProjectile
{
    public GameObject hitRangeEfx_ref;

    private float elapsedTime = 0.0f;

    private GameObject hitRangeEfx;

    private int owner = -1;

    private void Start()
    {
        object[] data = photonView.instantiationData;

        owner = (int)data[0];

        Velocity = new Vector3(0.0f, -speed);

        if(photonView.isMine)
            DisplayHitRange();
    }

    protected override void Update()
    {
        if (!photonView.isMine) return;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= lifetime)
        {
            PhotonNetwork.Destroy(hitRangeEfx);
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
                    if (objPhotonView.ownerId != owner)
                    {
                        objPhotonView.RPC("Pushed", objPhotonView.owner, (direction * hitForce));
                        objPhotonView.RPC("Damage", objPhotonView.owner, damage, -1);
                        // 타상연화
                    }
                }

                // 피격 이펙트
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
            else
            {
                objPhotonView.RPC("Pushed", PhotonTargets.MasterClient, (direction * hitForce));
            }
        }
        //photonView.RPC("PlayEndSound", PhotonTargets.All, null);

        PhotonNetwork.Instantiate("Prefabs/Tasang_Hit_fx", transform.position, transform.rotation, 0);
        PhotonNetwork.Destroy(hitRangeEfx);
        PhotonNetwork.Destroy(gameObject);
    }

    /// <summary>
    /// 타상연화가 떨어지면 피격되는 범위를 표시합니다.
    /// </summary>
    private void DisplayHitRange()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, 22.0f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);

        hitRangeEfx = PhotonNetwork.Instantiate("Prefabs/" + hitRangeEfx_ref.name, hit.point, Quaternion.identity, 0);
    }
}
