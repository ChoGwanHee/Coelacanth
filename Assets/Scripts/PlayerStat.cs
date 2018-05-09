using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerStat : Photon.PunBehaviour
{

    public int playerIndex;
    public bool onStage = true;     // 스테이지 상에서 살아있음, 번지 X
    public bool alive = true;       // 라이프가 1이상
    public string nickname;         // 닉네임
    public int maxHP = 100;         // 최대 HP
    public int curHP;               // 현재 HP
    public int life = 3;            // 남은 라이프

    [FMODUnity.EventRef]
    public string lossSound;
    

    private PlayerController pc;

    private void Awake()
    {
        curHP = maxHP;
        pc = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (GetComponent<PlayerDummy>()) return;

        Hashtable playerProperties = photonView.owner.CustomProperties;
        playerIndex = (int)playerProperties["PlayerIndex"];

        if (!photonView.isMine)
            UIManager._instance.ActiveOtherStatus(playerIndex);
    }

    [PunRPC]
    public void Damage(int dmg)
    {
        if (!onStage) return;

        if (curHP - dmg <= 0)
        {
            curHP = 0;
            if (pc != null)
                pc.Stun();
        }
        else
        {
            curHP -= dmg;
        }
    }

    public void LifeLoss()
    {
        life--;
        FMODUnity.RuntimeManager.PlayOneShot(lossSound);

        if(life <= 0)
        {
            // 라이프 다 없어짐
            alive = false;
            Debug.Log(gameObject.GetPhotonView().owner + " 사망");
        }

        if (!photonView.isMine)
        {
            SetOtherHeartUI(life);
        }
        else
        {
            UIManager._instance.userStatus.SetHeart(life);
        }
    }

    public void HPReset()
    {
        curHP = maxHP;
    }

    [PunRPC]
    public void SetOtherHeartUI(int num)
    {
        UIManager._instance.otherStatus[UIManager._instance.otherUserIndexLink[playerIndex]].SetHeart(num);
    }
}
