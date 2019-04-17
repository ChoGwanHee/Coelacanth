using UnityEngine;
using ServerModule;
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
    /// 점수변경 잠금
    /// </summary>
    public bool scoreLock = true;

    /// <summary>
    /// 플레이어가 준비중인지 여부
    /// </summary>
    public bool isReady = false;

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
    public delegate void OnReadyChangedDelegate(bool isReady);

    /// <summary>
    /// 플레이어의 점수가 바뀌었을 때 호출되는 델리게이트
    /// </summary>
    public OnScoreChangedDelegate onScoreChanged;

    public OnReadyChangedDelegate onReadyChanged;

    
    private PlayerController pc;
    public PlayerController PC
    {
        get { return pc; }
    }


    private void Awake()
    {
        curHP = maxHP;
        pc = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if(GameManagerPhoton._instance != null)
            GameManagerPhoton._instance.playerList.Add(this);

        if (GetComponent<PlayerDummy>()) return;

        UIScoreBoard scoreBoard = UIManager._instance.scoreBoard;


        // 닉네임 받아오기
        nickname = photonView.owner.NickName;

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

        onReadyChanged(GameManagerPhoton._instance.GetPlayerReady(photonView.ownerId));
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
        if(GameManagerPhoton._instance != null)
            GameManagerPhoton._instance.playerList.Remove(this);
    }

    /// <summary>
    /// 플레이어에게 피해를 줍니다.
    /// </summary>
    /// <param name="dmg">피해량</param>
    /// <param name="attackerIndex">공격자</param>
    [PunRPC]
    public void Damage(int dmg, int attackerIndex)
    {
        if (!onStage || pc.isStun || pc.isUnbeatable) return;

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

        if (attackerIndex != -1)
        {
            SetAttacker(attackerIndex);
        }
    }

    /// <summary>
    /// 플레이어에게 피해를 주고 화면을 흔듭니다.
    /// </summary>
    /// <param name="damage">피해량</param>
    /// <param name="shakeEventNum">카메라 쉐이크 이벤트 넘버</param>
    /// <param name="attackerIndex">공격자</param>
    [PunRPC]
    public void DamageShake(int damage, int shakeEventNum, int attackerIndex)
    {
        if (!onStage || pc.isStun || pc.isUnbeatable) return;

        ShakeEvent se = GameManagerPhoton._instance.damageShakeEvents[shakeEventNum];

        if (curHP - damage <= 0)
        {
            curHP = 0;
            if (pc != null)
                pc.ChangeState(PlayerAniState.Stun);
        }
        else
        {
            curHP -= damage;
        }

        if (attackerIndex != -1)
            SetAttacker(attackerIndex);

        // 카메라 쉐이크
        GameManagerPhoton._instance.cameraController.Shake(se.Amplitude, se.Duration);
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
                    PlayerStat attacker = GameManagerPhoton._instance.playerList[i];
                    attacker.AddScore(100);
                    attacker.photonView.RPC("DisplayKillIndicator", attacker.photonView.owner, pc.characterNum);
                    break;
                }
            }
            killApproval = false;
            lastAttacker = -1;
        }
        // 스스로 킬
        else
        {
            pc.isUnbeatable = false;
        }

        //사망자 점수 감산
        AddScore(-10);
    }

    public void AddScore(int score)
    {
        if(scoreLock || pc.isUnbeatable && score <= 0)
            return;

        if(Score + score < 0)
        {
            Score = 0;
        }
        else
        {
            ServerManager.Send(string.Format("SCORE:{0}:{1}:{2}", InstanceValue.Nickname, InstanceValue.ID, score));
            photonView.owner.AddScore(score);
        }
    }

    [PunRPC]
    private void DisplayKillIndicator(int characterNum)
    {
        UIManager._instance.killIndicator.SetIndicator(characterNum);
    }
}
