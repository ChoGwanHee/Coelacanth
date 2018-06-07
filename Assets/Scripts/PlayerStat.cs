using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 플레이어의 상태를 관리하는 클래스
/// </summary>
public class PlayerStat : Photon.PunBehaviour
{
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

    /// <summary>
    /// 플레이어의 점수
    /// </summary>
    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            if(value < 0)
            {
                score = 0;
            }
            else
            {
                score = value;
            }
            if(onScoreChanged != null)
                onScoreChanged(score);
        }
    }

    /// <summary>
    /// 킬 판정 여부
    /// </summary>
    private bool killApproval;

    /// <summary>
    /// 이 플레이어를 마지막으로 공격한 플레이어의 인덱스
    /// </summary>
    private int lastAttacker = -1;
    public int LastAttacker
    {
        get { return lastAttacker; }
        set { lastAttacker = value; }
    }

    /// <summary>
    /// 마지막 피격으로부터 지난 시간
    /// </summary>
    private float lastHitElapsedTime;

    public bool IsControlable
    {
        get { return pc.isControlable; }
        set { pc.isControlable = value; }
    }


    public delegate void OnScoreChangedDelegate(int newScore);

    /// <summary>
    /// 플레이어의 점수가 바뀌었을 때 호출되는 델리게이트
    /// </summary>
    public OnScoreChangedDelegate onScoreChanged;

    
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

        if (photonView.isMine)
        {
            UIManager._instance.chargingUI.target = transform;
            GameManagerPhoton._instance.cameraController.SetTarget(transform);

            UIManager._instance.scoreBoard.cells[0].SetPlayerStat(this);
        }
        else
        {
            for(int i=0; i<UIManager._instance.scoreBoard.cells.Length; i++)
            {
                if(UIManager._instance.scoreBoard.cells[i].IsNotRegist)
                {
                    UIManager._instance.scoreBoard.cells[i].SetPlayerStat(this);
                    break;
                }
            }
        }

        if (PhotonNetwork.isMasterClient)
        {
            GameManagerPhoton._instance.CheckFull();
        }
    }

    private void Update()
    {
        if (!photonView.isMine) return;

        if(killApproval)
        {
            lastHitElapsedTime += Time.deltaTime;
            if(lastHitElapsedTime >= 10.0f)
            {
                killApproval = false;
            }
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
    public void Damage(int dmg, int attackerIndex)
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

        if(attackerIndex != -1)
            SetAttacker(attackerIndex);
    }

    /// <summary>
    /// 체력을 재설정합니다.
    /// </summary>
    public void HPReset()
    {
        curHP = maxHP;
    }

    /// <summary>
    /// 공격자를 설정합니다.
    /// </summary>
    /// <param name="attackerIndex">공격자의 인덱스</param>
    public void SetAttacker(int attackerIndex)
    {
        lastAttacker = attackerIndex;
        lastHitElapsedTime = 0;
        killApproval = true;
    }

}
