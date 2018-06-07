using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
/// <summary>
/// 포톤을 사용한 게임 매니저 클래스
/// </summary>
public class GameManagerPhoton : Photon.PunBehaviour
{
    /// <summary>
    /// 싱글턴 인스턴스
    /// </summary>
    public static GameManagerPhoton _instance;


    /// <summary>
    /// 글로벌 판정 간격
    /// </summary>
    private float gameTick = 0.1f;
    public float GameTick
    {
        get { return gameTick; }
    }
    
    /// <summary>
    /// 플레이어가 생성되는 위치 배열
    /// </summary>
    public Transform[] playerGenPos = null;

    /// <summary>
    /// 커서 이미지
    /// </summary>
    public Texture2D cursorTex;

    /// <summary>
    /// 현재 게임의 상태
    /// </summary>
    public GameState currentState = GameState.Wait;

    /// <summary>
    /// 게임이 진행 중인지 여부
    /// </summary>
    private bool isPlaying;
    public bool IsPlaying
    {
        get { return isPlaying; }
    }

    /// <summary>
    /// 게임이 중단 중인지 여부
    /// </summary>
    private bool isStopping = false;

    /// <summary>
    /// 총 게임 진행 시간
    /// </summary>
    public float playTime;

    /// <summary>
    /// 남은 게임 시간
    /// </summary>
    private float remainGameTime;
    public float RemainGameTime
    {
        get { return remainGameTime; }
    }


    public int startPlayerCount = 4;


    [FMODUnity.EventRef]
    public string BGM;

    /// <summary>
    /// 현재 존재하는 플레이어의 리스트
    /// </summary>
    [HideInInspector]
    public List<PlayerStat> playerList = new List<PlayerStat>();


    /// <summary>
    /// 사망 이펙트
    /// </summary>
    public GameObject deadEfx_ref;


    [HideInInspector]
    public CameraController cameraController;
    [HideInInspector]
    public ItemManager itemManager;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        cameraController = Camera.main.transform.parent.parent.GetComponent<CameraController>();
        itemManager = GetComponent<ItemManager>();

        Cursor.SetCursor(cursorTex, new Vector2(44, 44), CursorMode.ForceSoftware);

        PhotonNetwork.isMessageQueueRunning = true;

        Hashtable customProperties = PhotonNetwork.player.CustomProperties;
        int playerIndex = (int)customProperties["PlayerIndex"];
        CreatePlayer(playerIndex, playerGenPos[playerIndex].position);

        //FMODUnity.RuntimeManager.PlayOneShot(BGM);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(remainGameTime);
        }
        else
        {
            this.remainGameTime = (float)stream.ReceiveNext();
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("플레이어 접속:" + player.NickName);
        
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Hashtable roomProperties = PhotonNetwork.room.CustomProperties;
        Hashtable playerProperties = otherPlayer.CustomProperties;

        if (PhotonNetwork.isMasterClient)
        {
            roomProperties["PlayerEnter" + (int)playerProperties["PlayerIndex"]] = false;
            PhotonNetwork.room.SetCustomProperties(roomProperties);
        }

        Debug.Log("플레이어 접속 종료:" + otherPlayer.NickName);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("플레이어 퇴장:" + PhotonNetwork.player.NickName);
    }

    /// <summary>
    /// 플레이어를 생성합니다.
    /// </summary>
    public void CreatePlayer(int characterIndex, Vector3 spawnPosition, bool isRandom = false)
    {
        string prefabName;

        switch (characterIndex)
        {
            case 0:
                prefabName = "Dahong_0521";
                break;
            case 1:
                prefabName = "MingMing_0516";
                break;
            case 2:
                prefabName = "ShoSho_0605_2";
                break;
            case 3:
                prefabName = "Juhwang_0426";
                break;
            default:
                prefabName = "Dahong_0424";
                break;
        }

        PhotonNetwork.Instantiate("Prefabs/Character/"+prefabName, spawnPosition, Quaternion.identity, 0);
        Debug.Log("플레이어 생성");
    }

    /// <summary>
    /// 플레이어를 리스폰 위치로 이동 시킵니다.
    /// </summary>
    /// <param name="playerTF">이동시킬 플레이어</param>
    public void RespawnPlayer(Transform playerTF)
    {
        Hashtable playerProperties = PhotonNetwork.player.CustomProperties;
        Vector3 pos = playerGenPos[(int)playerProperties["PlayerIndex"]].position;
        pos.y += 0.5f;

        playerTF.position = pos;
    }

    /// <summary>
    /// 4명의 플레이어들이 모두 들어왔는지 확인합니다.
    /// </summary>
    public void CheckFull()
    {
        if(PhotonNetwork.room.PlayerCount >= startPlayerCount)
        {
            // 게임 시작
            photonView.RPC("RunGameEvent", PhotonTargets.All, (int)GameEvent.GameStart);
        }
    }

    private void EnterState(GameState state)
    {
        switch(state)
        {
            case GameState.Wait:
                break;
            case GameState.Playing:
                if (PhotonNetwork.isMasterClient)
                {
                    MapManager._instance.StartMapFacilities();
                    itemManager.active = true;
                    remainGameTime = playTime;
                    StartCoroutine(GameLoop());
                }
                isPlaying = true;
                SetPlayerActive(true);

                break;
            case GameState.Result:
                isPlaying = false;
                SetPlayerActive(false);
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        EnterState(newState);
    }

    [PunRPC]
    public void RunGameEvent(int eventNum)
    {
        switch(eventNum)
        {
            case (int)GameEvent.GameStart:
                StartCoroutine(GameCountProcess(true));
                break;
            case (int)GameEvent.GameStop:
                StartCoroutine(GameCountProcess(false));
                break;
        }
        
    }

    private IEnumerator GameCountProcess(bool isStart)
    {
        UIManager._instance.counterAnim.gameObject.SetActive(true);
        UIManager._instance.counterAnim.SetInteger("Count", 3);
        yield return new WaitForSeconds(1.0f);
        UIManager._instance.counterAnim.SetInteger("Count", 2);
        yield return new WaitForSeconds(1.0f);
        UIManager._instance.counterAnim.SetInteger("Count", 1);
        yield return new WaitForSeconds(1.0f);
        UIManager._instance.counterAnim.gameObject.SetActive(false);

        if (isStart)
        {
            ChangeState(GameState.Playing);
        }
        else
        {
            ChangeState(GameState.Result);
        }
        
    }

    private void SetPlayerActive(bool active)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].photonView.isMine)
            {
                playerList[i].IsControlable = active;
                break;
            }
        }
    }


    private IEnumerator GameLoop()
    {
        while (remainGameTime > 0)
        {
            remainGameTime -= Time.deltaTime;

            if (!isStopping && remainGameTime <= 4)
            {
                isStopping = true;
                photonView.RPC("RunGameEvent", PhotonTargets.All, (int)GameEvent.GameStop);
            }
            yield return null;
        }

    }

}
