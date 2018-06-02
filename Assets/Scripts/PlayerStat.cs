using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 플레이어의 상태를 관리하는 클래스
/// </summary>
public class PlayerStat : Photon.PunBehaviour
{
    /// <summary>
    /// 플레이어의 고유 인덱스
    /// </summary>
    public int playerIndex;

    /// <summary>
    /// 플레이어가 스테이지 상에서 살아있는지 여부
    /// </summary>
    public bool onStage = true;

    /// <summary>
    /// 플레이어가 게임에 참여하고 있는지 여부
    /// </summary>
    public bool alive = true;

    /// <summary>
    /// 플레이어의 닉네임
    /// </summary>
    public string nickname;

    /// <summary>
    /// 최대 체력
    /// </summary>
    public int maxHP = 100;

    /// <summary>
    /// 현재 체력
    /// </summary>
    public int curHP;

    
    private PlayerController pc;


    private void Awake()
    {
        curHP = maxHP;
        pc = GetComponent<PlayerController>();
    }

    private void Start()
    {
        GameManagerPhoton._instance.playerList.Add(this);

        if (GetComponent<PlayerDummy>()) return;

        Hashtable playerProperties = photonView.owner.CustomProperties;
        playerIndex = (int)playerProperties["PlayerIndex"];

        if (photonView.isMine)
        {
            UIManager._instance.chargingUI.target = transform;
            GameManagerPhoton._instance.cameraController.SetTarget(transform);
        }
    }

    private void OnDestroy()
    {
        GameManagerPhoton._instance.playerList.Remove(this);
    }

    /// <summary>
    /// 플레이어에게 피해를 줍니다.
    /// </summary>
    /// <param name="dmg">피해량</param>
    [PunRPC]
    public void Damage(int dmg)
    {
        if (!onStage || pc.isStun) return;

        if (curHP - dmg <= 0)
        {
            curHP = 0;
            if (pc != null)
                pc.ChangeState(PlayerAniState.Stun);
        }
        else
        {
            curHP -= dmg;
        }
    }

    /// <summary>
    /// 체력을 재설정합니다.
    /// </summary>
    public void HPReset()
    {
        curHP = maxHP;
    }
}
