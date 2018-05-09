using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireworkExecuter : Photon.PunBehaviour {

    public Transform firePoint;
    public Firework curFirework;
    public Firework defaultFirework;        // 기본 폭죽(주먹)

    private float elapsedTime;
    public int ammo;
    private bool fireEnable = true;

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
        elapsedTime = 0f;
        pc.SetAnimParam("FireworkType", (int)curFirework.fwType);
        pc.ChangeState(PlayerAniState.Attack);

        // -1은 탄환 무한이기 때문에 예외처리
        if (ammo != -1)
        {
            ammo--;
            ChangeAmmoUI(ammo);
        }

    }

    public void Execute()
    {
        if (!photonView.isMine) return;

        curFirework.Execute(this);
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
        curFirework = GameManagerPhoton._instance.itemManager.GetFireworkItem(tableIndex, itemIndex);
        Initialize();

        if (photonView.isMine)
        {
            UIManager._instance.userStatus.SetWeaponImg(curFirework.uiSprite);
            ChangeAmmoUI(ammo);
        }
        

        Debug.Log(" 무기교체:" + curFirework.GetType() + "\n교체자:" + photonView.owner);
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
}
