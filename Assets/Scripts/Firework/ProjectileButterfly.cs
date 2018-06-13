using UnityEngine;

/// <summary>
/// 나비 폭죽 투사체 클래스
/// </summary>
public class ProjectileButterfly : BaseProjectile
{
    /// <summary>
    /// 유도될 표적
    /// </summary>
    public Transform target;        // private으로 변경 예정

    /// <summary>
    /// 초당 최대 회전각
    /// </summary>
    public float rotateAngle;


    protected override void Update()
    {
        if (!photonView.isMine) return;

        // 수명이 다 되면 폭발
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Explosion();
        }

        Rotate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;
        if (other.CompareTag("Item")) return;

        // 부딪힌 오브젝트가 나비 폭죽 투사체일 경우 동일한 ID를 가지고 있으면 터지지 않게 함
        ProjectileButterfly otherObject = other.GetComponent<ProjectileButterfly>();
        if (otherObject != null)
        {
            if (photonView.ownerId != otherObject.photonView.ownerId)
            {
                Explosion();
            }
        }
        else
        {
            Explosion();
        }
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

            effectedObjects[i].gameObject.GetPhotonView().RPC("Pushed", PhotonTargets.All, (direction * hitForce));

            if (effectedObjects[i].CompareTag("Player"))
            {
                effectedObjects[i].gameObject.GetPhotonView().RPC("Damage", PhotonTargets.All, damage, photonView.ownerId);
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);

                // 점수 처리
                if (effectedObjects[i].gameObject.GetPhotonView().ownerId == photonView.ownerId)
                {
                    // 본인 피격
                    effectedObjects[i].GetComponent<PlayerStat>().AddScore(-20);
                }
                else
                {
                    // 다른 사람 피격
                    effectedObjects[i].GetComponent<PlayerStat>().AddScore(-10);
                    GameManagerPhoton._instance.GetPlayerByOwnerId(photonView.ownerId).AddScore(gainScore);
                }
            }
        }

        photonView.RPC("PlayEndSound", PhotonTargets.All, null);

        PhotonNetwork.Instantiate("Prefabs/nabi_firework_Hit_fx", transform.position, transform.rotation, 0);
        PhotonNetwork.Destroy(gameObject);
    }

    /// <summary>
    /// 표적을 향해 회전합니다.
    /// </summary>
    private void Rotate()
    {
        if (target == null) return;

        Vector3 direction = Vector3.Scale((target.position - transform.position), new Vector3(1, 0, 1)).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateAngle * Time.deltaTime);

        rb.velocity = transform.forward * speed;
    }

    /// <summary>
    /// 무작위로 자신을 제외한 플레이어 중 한 명을 표적으로 지정합니다.
    /// </summary>
    /// <param name="exclusive">검색에서 제외시킬 인덱스 (자신의 인덱스)</param>
    public void SearchTarget(int exclusive)
    {
        int playerCount = GameManagerPhoton._instance.playerList.Count;
        int randomIndex;

        // 현재 플레이어가 2명 미만이면 리턴
        if (playerCount < 2) return;

        int[] indexArray = new int[playerCount];
        int curIndex = 0;

        for (int i=0; i< playerCount; i++)
        {
            // 제외시킬 인덱스는 통과
            if (i == exclusive) continue;

            if (GameManagerPhoton._instance.playerList[i].onStage)
            {
                indexArray[curIndex++] = i;
            }
        }

        // 현재 살아 있는 플레이어가 2명 미만이면 리턴
        if(curIndex < 1)
            return;

        randomIndex = Random.Range(0, curIndex);
        Debug.Log(randomIndex + ", "+ GameManagerPhoton._instance.playerList[randomIndex]);

        target = GameManagerPhoton._instance.playerList[indexArray[randomIndex]].transform;
    }
}
