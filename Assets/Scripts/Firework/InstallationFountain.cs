using UnityEngine;

/// <summary>
/// 분수 폭죽 설치물 클래스
/// </summary>
public class InstallationFountain : BaseInstallation
{
    /// <summary>
    /// 공격력
    /// </summary>
    [HideInInspector]
    public int damage;

    /// <summary>
    /// 넉백력
    /// </summary>
    [HideInInspector]
    public float hitForce;

    /// <summary>
    /// 공격 반경
    /// </summary>
    [HideInInspector]
    public float hitRadius;

    /// <summary>
    /// 활성화 여부
    /// </summary>
    private bool enable = false;

    /// <summary>
    /// 활성화까지 걸리는 시간
    /// </summary>
    public float enableTime = 1.2f;

    /// <summary>
    /// 판정 간격
    /// </summary>
    private float tickTime;

    /// <summary>
    /// 마지막 판정 후 지난 시간
    /// </summary>
    private float tickElapsedTime = 0;

    /// <summary>
    /// 날아가는 도중 재생되는 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string duringSound;

    private ParticleSystem particle;

    private LayerMask dynamicObjMask;


    private void Start()
    {
        // 게임매니저에서 글로벌 틱 시간을 가져옴
        tickTime = GameManagerPhoton._instance.gameTick;
        particle = GetComponentInChildren<ParticleSystem>();
        dynamicObjMask = LayerMask.GetMask("DynamicObject");
    }

    protected override void Update()
    {
        base.Update();


        if (enable && photonView.isMine)
        {
            if (tickElapsedTime >= tickTime)
            {
                Sprinkle();
                tickElapsedTime = 0;
            }
            else
            {
                tickElapsedTime += Time.deltaTime;
            }
        }
        else
        {
            if (elapsedTime >= enableTime)
            {
                enable = true;
                elapsedTime = 0;
                particle.Play();
                FMODUnity.RuntimeManager.PlayOneShot(duringSound);
            }
        }
    }

    /// <summary>
    /// 일정 반경 안의 물체를 밀어내고 피해를 줍니다.
    /// </summary>
    private void Sprinkle()
    {
        Collider[] effectedObjects = Physics.OverlapSphere(transform.position, hitRadius, dynamicObjMask);

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            Vector3 direction = Vector3.Scale(effectedObjects[i].transform.position - transform.position, new Vector3(1, 0, 1)).normalized;

            effectedObjects[i].gameObject.GetPhotonView().RPC("Pushed", PhotonTargets.All, (direction * hitForce));

            if (effectedObjects[i].CompareTag("Player"))
            {
                effectedObjects[i].gameObject.GetPhotonView().RPC("Damage", PhotonTargets.All, damage);
                Vector3 centerPos = transform.position;
                centerPos.y += 0.7f;
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(centerPos);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
        }
    }
}
