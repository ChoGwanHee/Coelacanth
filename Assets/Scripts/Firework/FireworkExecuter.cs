using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireworkExecuter : Photon.PunBehaviour {

    public Transform firePoint;             // 투사체가 생성되는 위치
    public Firework defaultFirework;        // 기본 폭죽(주먹)
    public Firework curFirework;            // 현재 장착 중인 폭죽
    public Firework newFirework;            // 곧 교체될 폭죽

    private float elapsedTime;              // 마지막 발사 후 지난 시간
    public int ammo;                        // 남은 탄약수
    private bool fireEnable = true;         // 발사 가능 여부
    public bool replaceable = true;        // 폭죽 교체 가능 여부
    public bool charging = false;           // 충전 중인지 여부

    private PlayerController pc;
    private PlayerStat stat;


    private void Awake()
    {
        elapsedTime = 0f;
    }

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStat>();
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

    // 갖고 있는 폭죽이 바뀐 후에 호출
    public void Initialize()
    {
        ammo = curFirework.capacity;
    }

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
        ChangeAmmoUI(ammo);
    }

    public void CheckRunOutAmmo()
    {
        // -1은 탄환 무한이기 때문에 예외처리
        if (ammo != -1)
        {
            // 남은 탄환이 0이면
            if (ammo == 0)
            {
                ChangeFirework(0, 0);
            }
        }
    }

    [PunRPC]
    public void ChangeFirework(int tableIndex, int itemIndex)
    {
        newFirework = GameManagerPhoton._instance.itemManager.GetFireworkItem(tableIndex, itemIndex);

        CheckFireworkChanged();
    }

    public void ChangeAmmoUI(int num)
    {
        UIManager._instance.userStatus.SetCount(num);
    }

    [PunRPC]
    public void PlayStartSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(curFirework.startSound);
    }

    public void CheckFireworkChanged()
    {
        if (newFirework != null && replaceable) {
            curFirework = newFirework;
            newFirework = null;
            Initialize();

            if (photonView.isMine)
            {
                UIManager._instance.userStatus.SetWeaponImg(curFirework.uiSprite);
                ChangeAmmoUI(ammo);
            }

            Debug.Log(" 무기교체:" + curFirework.GetType() + "\n교체자:" + photonView.owner);
        }
    }
}
