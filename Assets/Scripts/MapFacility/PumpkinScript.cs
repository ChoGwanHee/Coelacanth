using UnityEngine;

public class PumpkinScript : Photon.PunBehaviour
{
    /// <summary>
    /// 호박이 생긴 후 대기시간
    /// </summary>
    public float stayTime;

    /// <summary>
    /// 대기중인지 여부
    /// </summary>
    private bool isStay = false;

    /// <summary>
    /// 대기 시작으로 부터 지난 시간
    /// </summary>
    private float elapsedTime = 0.0f;

    /// <summary>
    /// 폭발시 피격 범위
    /// </summary>
    public float hitRadius;

    /// <summary>
    /// 넉백력
    /// </summary>
    public float hitForce;

    /// <summary>
    /// 폭발 이펙트 레퍼런스
    /// </summary>
    public GameObject hitEfx_ref;


    [FMODUnity.EventRef]
    public string upSound;

    [FMODUnity.EventRef]
    public string boomSound;

    [FMODUnity.EventRef]
    public string laughSound;

    private Animator animator;

    private int dynamicObjMask;
    private bool laughOnce = false;


    private void Awake()
    {
        dynamicObjMask = LayerMask.GetMask("DynamicObject");
        animator = GetComponent<Animator>();
    }

    void Update () {
		if(isStay)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= stayTime)
            {
                animator.SetInteger("State", 2);
                isStay = false;
            }
        }
	}

    private void Explosion()
    {
        // 사운드 재생
        FMODUnity.RuntimeManager.PlayOneShot(boomSound);

        if (!photonView.isMine) return;

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

        PhotonNetwork.Instantiate("Prefabs/" + hitEfx_ref.name, transform.position, transform.rotation, 0);
    }

    private void StayPumpkin()
    {
        isStay = true;
        animator.SetInteger("State", 1);
    }

    private void DestroyPumpkin()
    {
        if (photonView.isMine)
            PhotonNetwork.Destroy(gameObject);
    }

    private void PlayUpSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(upSound);
    }

    private void PlayLaughSound()
    {
        if (laughOnce) return;

        FMODUnity.RuntimeManager.PlayOneShot(laughSound);
        laughOnce = true;
    }
}
