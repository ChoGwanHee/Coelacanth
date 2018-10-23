using System.Collections;
using System.Collections.Generic;
using ServerModule;
using UnityEngine;

/// <summary>
/// 플레이어가 갖고 있는 폭죽을 작동, 발사, 관리하는 클래스
/// </summary>
public class FireworkExecuter : Photon.PunBehaviour {

    public Transform firePoint;             // 투사체가 생성되는 위치
    public Firework defaultFirework;        // 기본 폭죽(주먹)
    public Firework curFirework;            // 현재 장착 중인 폭죽
    public Firework newFirework;            // 곧 교체될 폭죽

    private float elapsedTime;              // 마지막 발사 후 지난 시간
    public int ammo;                        // 남은 탄약수
    private bool fireEnable = true;         // 발사 가능 여부
    public bool replaceable = true;         // 폭죽 교체 가능 여부
    public bool charging = false;           // 충전 중인지 여부

    /// <summary>
    /// 데미지 계수
    /// </summary>
    public float damageFactor = 1.0f;

    /// <summary>
    /// 넉백력 계수
    /// </summary>
    public float forceFactor = 1.0f;

    /// <summary>
    /// 아이템을 획득할 때 출력되는 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string getSound;


    public delegate void OnFireworkChangedDelegate(Firework newFirework);
    public delegate void OnFireworkAmmoChangedDelegate(int num);

    /// <summary>
    /// 폭죽이 바뀌었을 때 호출되는 델리게이트 입니다.
    /// </summary>
    public OnFireworkChangedDelegate onFireworkChanged;

    /// <summary>
    /// 남은 탄약수가 바뀌었을 때 호출되는 델리게이트 입니다.
    /// </summary>
    public OnFireworkAmmoChangedDelegate onFireworkAmmoChanged;
    

    private PlayerController pc;
    private PlayerStat stat;
    public PlayerStat Stat
    {
        get { return stat; }
    }


    private void Awake()
    {
        elapsedTime = 0f;
    }

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStat>();

        onFireworkChanged = new OnFireworkChangedDelegate(pc.CheckHandObject);

        if (photonView.isMine)
        {
            if(UIManager._instance != null)
            {
                onFireworkChanged += UIManager._instance.userStatus.ChangeWeapon;
                onFireworkAmmoChanged = new OnFireworkAmmoChangedDelegate(UIManager._instance.ammoCounter.SetAmount);
            }
        }
    }

    private void Update()
    {
        if (curFirework == null) return;

        if (!fireEnable)
        {
            if (elapsedTime >= curFirework.coolDown)
            {
                fireEnable = true;
            }
            else
            {
                elapsedTime += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 갖고 있는 폭죽이 바뀐뒤에 호출하는 초기화 함수
    /// </summary>
    public void Initialize()
    {
        ammo = curFirework.capacity;
        if (onFireworkAmmoChanged != null)
            onFireworkAmmoChanged(ammo);
    }

    /// <summary>
    /// 폭죽 작동 시작
    /// </summary>
    public void Trigger()
    {
        if (!fireEnable) return;
        if (curFirework == null)
        {
            Debug.Log("curFirework가 null입니다");
            return;
        }

        fireEnable = false;
        replaceable = false;
        elapsedTime = 0f;
        pc.SetAnimParam("FireworkType", (int)curFirework.fwType);
        pc.ChangeState(PlayerAniState.Attack);
    }

    /// <summary>
    /// 폭죽 발사
    /// </summary>
    public void Execute()
    {
        if (!photonView.isMine) return;

        curFirework.Execute(this);
    }


    /// <summary>
    /// 남은 탄약 감소
    /// </summary>
    public void DecreaseAmmo()
    {
        ammo--;
        if (onFireworkAmmoChanged != null)
            onFireworkAmmoChanged(ammo);
    }

    /// <summary>
    /// 탄환이 다 떨어졌는지 체크합니다.
    /// </summary>
    public void CheckRunOutAmmo()
    {
        // -1은 탄환 무한이기 때문에 예외처리
        if (ammo != -1)
        {
            // 남은 탄환이 0이면
            if (ammo == 0)
            {
                photonView.RPC("ChangeFirework", PhotonTargets.All, 0, 0);
            }
        }
    }

    /// <summary>
    /// 갖고 있는 폭죽을 바꿉니다.
    /// </summary>
    /// <param name="tableIndex">아이템 테이플의 인덱스</param>
    /// <param name="itemIndex">아이템의 인덱스</param>
    [PunRPC]
    public void ChangeFirework(int tableIndex, int itemIndex)
    {
        newFirework = GameManagerPhoton._instance.itemManager.GetItem(tableIndex, itemIndex) as Firework;
        CheckFireworkChanged();
        if(tableIndex != 0 && itemIndex != 0)
            PlayGetSound();
    }

    /// <summary>
    /// 갖고 있는 폭죽이 바뀌었는지 체크합니다.
    /// </summary>
    public void CheckFireworkChanged()
    {
        if (newFirework != null && replaceable) {
            curFirework = newFirework;
            newFirework = null;
            Initialize();

            if(onFireworkChanged != null)
                onFireworkChanged(curFirework);
            if (onFireworkAmmoChanged != null)
                onFireworkAmmoChanged(ammo);

            if (photonView.isMine)
            {
                ServerManager.Send(string.Format("WEAPONCHANGE:{0}:{1}:{2}:{3}", InstanceValue.Nickname, InstanceValue.ID, curFirework.GetType(), photonView.owner));
                Debug.Log(" 무기교체:" + curFirework.GetType() + "\n교체자:" + photonView.owner + ":" + photonView.ownerId);
            }
        }
    }

    private void PlayGetSound()
    {
        if (!photonView.isMine) return;

        FMODUnity.RuntimeManager.PlayOneShot(getSound);
    }
}
