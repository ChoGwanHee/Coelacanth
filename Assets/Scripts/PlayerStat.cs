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
    /// 캐릭터 초상화
    /// </summary>
    public Sprite characterPicture;

    /// <summary>
    /// 플레이어의 점수
    /// </summary>
    //private int score;
    /*public int Score
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
    }*/
    public int Score
    {
        get { return photonView.owner.GetScore(); }
        set
        {
            if(value < 0)
            {
                photonView.owner.SetScore(0);
            }
            else
            {
                photonView.owner.SetScore(value);
            }
        }
    }


    /// <summary>
    /// 킬 판정 여부
    /// </summary>
    private bool killApproval;

    /// <summary>
    /// 킬 판정 유효 시간
    /// </summary>
    private float killApprovalTime = 10.0f;

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

        UIScoreBoard scoreBoard = UIManager._instance.scoreBoard;

        if (photonView.isMine)
        {
            GameManagerPhoton._instance.cameraController.SetTarget(transform);
            UIManager._instance.chargingUI.target = transform;
            scoreBoard.cells[0].SetPlayerStat(this);
            scoreBoard.cells[0].SetPlayerPicture(characterPicture);
        }
        else
        {
            for(int i=0; i<UIManager._instance.scoreBoard.cells.Length; i++)
            {
                if(!scoreBoard.cells[i].isMine && scoreBoard.cells[i].IsNotRegist)
                {
                    scoreBoard.cells[i].SetPlayerStat(this);
                    break;
                }
            }
        }

        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log(PhotonNetwork.isMasterClient + ", " + photonView.owner);
        }
    }

    private void Update()
    {
        if (!photonView.isMine) return;

        if(killApproval)
        {
            lastHitElapsedTime += Time.deltaTime;
            if(lastHitElapsedTime >= killApprovalTime)
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

    [PunRPC]
    public void DamageShake(int damageEventNum, int attackerIndex)
    {
        if (!onStage || pc.isStun) return;

        DamageShakeEvent de = GameManagerPhoton._instance.damageShakeEvents[damageEventNum];

        if (curHP - de.Damage <= 0)
        {
            curHP = 0;
            if (pc != null)
                pc.ChangeState(PlayerAniState.Stun);
        }
        else
        {
            curHP -= de.Damage;
        }

        if (attackerIndex != -1)
            SetAttacker(attackerIndex);

        // 카메라 쉐이크
        GameManagerPhoton._instance.cameraController.Shake(de.Amplitude, de.Duration);
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

    /// <summary>
    /// 킬 점수 판정을 합니다.
    /// </summary>
    public void KillScoring()
    {
        if (!photonView.isMine) return;

        // 다른 사람에 의해 킬
        if (killApproval && lastAttacker != photonView.ownerId)
        {
            // 공격자 점수 가산
            for(int i=0; i<GameManagerPhoton._instance.playerList.Count; i++)
            {
                if(GameManagerPhoton._instance.playerList[i].photonView.ownerId == lastAttacker)
                {
                    GameManagerPhoton._instance.playerList[i].AddScore(100);
                    break;
                }
            }

            // 사망자 점수 감산
            AddScore(-30);

            killApproval = false;
            lastAttacker = -1;
        }
        // 스스로 킬
        else
        {
            // 사망자 점수 감산
            AddScore(-50);
        }
    }

    public void AddScore(int score)
    {
        if(Score + score < 0)
        {
            Score = 0;
        }
        else
        {
            photonView.owner.AddScore(score);
        }
    }
}
