using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool isSceneMoving = false;


    [FMODUnity.EventRef]
    public string BGM;
    FMOD.Studio.EventInstance BGMEvent;

    [FMODUnity.EventRef]
    public string resultBGM;
    FMOD.Studio.EventInstance resultBGMEvent;


    /// <summary>
    /// 현재 존재하는 플레이어의 리스트
    /// </summary>
    [HideInInspector]
    public List<PlayerStat> playerList = new List<PlayerStat>();

    private bool[] playerEnter;


    /// <summary>
    /// 사망 이펙트
    /// </summary>
    public GameObject deadEfx_ref;

    /// <summary>
    /// 캐릭터 정면뷰로 확대시 배경에서 터지는 폭죽 이펙트
    /// </summary>
    public GameObject[] backgroundFireworks;


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
        remainGameTime = playTime;

        Cursor.SetCursor(cursorTex, new Vector2(0, 0), CursorMode.ForceSoftware);

        playerEnter = new bool[(PhotonNetwork.room.MaxPlayers)];

        PhotonNetwork.isMessageQueueRunning = true;


        Hashtable customProperties = PhotonNetwork.player.CustomProperties;
        int playerIndex = (int)customProperties["PlayerIndex"];
        CreatePlayer(playerIndex, playerGenPos[playerIndex].position);

        // BGM
        BGMEvent = FMODUnity.RuntimeManager.CreateInstance(BGM);
        BGMEvent.start();

        resultBGMEvent = FMODUnity.RuntimeManager.CreateInstance(resultBGM);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(remainGameTime);
            stream.SendNext(playerEnter);
        }
        else
        {
            this.remainGameTime = (float)stream.ReceiveNext();
            this.playerEnter = (bool[])stream.ReceiveNext();
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

    [PunRPC]
    private void AllowCreatePlayer(int characterIndex)
    {
        CreatePlayer(characterIndex, playerGenPos[characterIndex].position);
    }

    /// <summary>
    /// 플레이어를 생성합니다.
    /// </summary>
    public void CreatePlayer(int characterIndex, Vector3 spawnPosition)
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
                prefabName = "CuChen_0612";
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
        int random = Random.Range(0, 4);
        Vector3 pos = playerGenPos[random].position;
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
            GameStartRequest();
        }
    }

    /// <summary>
    /// 게임 시작을 요청합니다.
    /// </summary>
    /// <returns>성공 여부를 반환합니다.</returns>
    public bool GameStartRequest()
    {
        if (!photonView.isMine)
        {
            Debug.LogWarning("게임을 시작하려면 방장이어야 합니다.");
            return false;
        }

        if(isPlaying)
        {
            Debug.LogWarning("게임이 이미 시작 됐습니다.");
            return false;
        }

        // 게임 시작
        photonView.RPC("RunGameEvent", PhotonTargets.All, (int)GameEvent.GameStart);
        return true;
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
                    StartCoroutine(GameLoop());
                }
                isPlaying = true;
                SetPlayerActive(true);

                break;
            case GameState.Result:
                if (PhotonNetwork.isMasterClient)
                {
                    MapManager._instance.StopAllCoroutines();
                    itemManager.active = false;

                    int focusedPlayerOwnerId = GetHighScorePlayerOwnerId();
                    photonView.RPC("Spotlight", PhotonTargets.All, focusedPlayerOwnerId);
                }
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

    /// <summary>
    /// 가장 점수가 높은 플레이어의 ownerId를 얻어옵니다.
    /// </summary>
    /// <returns>플레이어 인덱스</returns>
    public int GetHighScorePlayerOwnerId()
    {
        if (playerList.Count < 1) return -1;

        PlayerStat highScorePlayer = playerList[0];
        int highScore = playerList[0].Score;

        for(int i=1; i<playerList.Count; i++)
        {
            if (highScore <= playerList[i].Score)
            {
                highScore = playerList[i].Score;
                highScorePlayer = playerList[i];
            }
        }

        return highScorePlayer.photonView.ownerId;
    }

    /// <summary>
    /// ownerId로 플레이어를 찾습니다.
    /// </summary>
    /// <param name="findOwnerId"></param>
    /// <returns></returns>
    public PlayerStat GetPlayerByOwnerId(int findOwnerId)
    {
        for(int i=0; i<playerList.Count; i++)
        {
            if(playerList[i].photonView.ownerId == findOwnerId)
            {
                return playerList[i];
            }
        }

        return null;
    }

    [PunRPC]
    private void Spotlight(int spotlightOwnerId)
    {
        PlayerController focusedPlayer = GetPlayerByOwnerId(spotlightOwnerId).GetComponent<PlayerController>();

        focusedPlayer.ChangeState(PlayerAniState.Idle);
        focusedPlayer.TurnToScreen();
        cameraController.ChangeMode(CameraMode.FrontView);
        cameraController.SetTarget(focusedPlayer.transform);
        UIManager._instance.ChangeScreen(UIManager.ScreenType.Result);
        UIManager._instance.resultScoreBoard.CalcResult();
        UIManager._instance.resultEmotion.SetPlayerController(focusedPlayer);
        if(focusedPlayer.photonView.isMine)
            UIManager._instance.resultEmotion.gameObject.SetActive(true);


        // 시야를 방해하는 오브젝트들 끄기
        Collider[] colls = Physics.OverlapBox(focusedPlayer.transform.position + new Vector3(0, 4.0f, -2f), new Vector3(1.0f, 4.0f, 2.0f), Quaternion.identity, LayerMask.GetMask("DynamicObject", "SubObject", "TableTopObject","Item"));
        for(int i=0; i<colls.Length; i++)
        {
            if (colls[i].CompareTag("Player")) continue;

            colls[i].gameObject.SetActive(false);
            Debug.Log(colls[i].gameObject);
        }

        
        // 배경의 폭죽 켜기
        for(int i = 0; i < backgroundFireworks.Length; i++)
        {
            backgroundFireworks[i].SetActive(true);
        }

        // 배경음악 재생
        BGMEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        resultBGMEvent.start();

        // 캐릭터 승리대사 재생
        focusedPlayer.PlayVoiceSound("Victory");
    }

    public void StartLoadTitleScene()
    {
        if(!isSceneMoving)
        {
            PhotonNetwork.Disconnect();
            StartCoroutine(LoadTitleScene());
            isSceneMoving = true;
        }
        else
        {
            Debug.LogWarning("이미 작동 중 입니다.");
        }
        
    }

    private IEnumerator LoadTitleScene()
    {
        // 게임씬을 완벽하게 로딩 후 씬을 변경한다
        AsyncOperation oper = SceneManager.LoadSceneAsync("Title");

        yield return oper; // 로딩이 완료될때까지 대기 한다
    }
}
