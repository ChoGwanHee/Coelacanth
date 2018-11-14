using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using ServerModule;
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
    /// 시간 진행 활성화
    /// </summary>
    public bool timeProgress = true;


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

    private bool[] playerReady = null;

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

    /// <summary>
    /// 게임이 자동으로 시작되는 플레이어 숫자
    /// </summary>
    public int startPlayerCount = 4;

    /// <summary>
    /// 미리 정의 되어 있는 데미지 이벤트들
    /// </summary>
    public ShakeEvent[] damageShakeEvents;

    /// <summary>
    /// 씬 이동 중인지 여부
    /// </summary>
    private bool isSceneMoving = false;



    /// <summary>
    /// 게임 배경음악
    /// </summary>
    [FMODUnity.EventRef]
    public string BGM;

    /// <summary>
    /// 결과화면 배경음악
    /// </summary>
    [FMODUnity.EventRef]
    public string resultBGM;

    /// <summary>
    /// 배경에서 터지는 폭죽 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string backgroundFirework;
    FMOD.Studio.EventInstance backgroundFireworkEvent;


    public bool backgroundFireworkSoundEnable;


    [FMODUnity.EventRef]
    public string readySound;

    [FMODUnity.EventRef]
    public string unreadySound;



    /// <summary>
    /// 현재 존재하는 플레이어의 리스트
    /// </summary>
    [HideInInspector]
    public List<PlayerStat> playerList = new List<PlayerStat>();

    private int[] playerEnter;

    /// <summary>
    /// 캐릭터 정면뷰로 확대시 배경에서 터지는 폭죽 이펙트
    /// </summary>
    public GameObject[] backgroundFireworks;

    public GameObject tempStartButton;


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

        // 커서 설정
        //Cursor.SetCursor(cursorTex, new Vector2(0, 0), CursorMode.ForceSoftware);

        playerEnter = new int[(PhotonNetwork.room.MaxPlayers)];
        playerReady = new bool[(PhotonNetwork.room.MaxPlayers)];

        PhotonNetwork.isMessageQueueRunning = true;
        
        if(PhotonNetwork.isMasterClient)
        {
            CreatePlayer(0, playerGenPos[0].position);
            
            playerEnter[0] = PhotonNetwork.player.ID;
            for (int i = 1; i < playerEnter.Length; i++)
            {
                playerEnter[i] = -1;
            }

            for(int i=0; i<playerReady.Length; i++)
            {
                playerReady[i] = false;
            }
        }

        UIManager._instance.readyInfo.SetActive(true);

        SoundManager._instance.SetBGM(BGM);

        if (backgroundFireworkSoundEnable)
        {
            backgroundFireworkEvent = FMODUnity.RuntimeManager.CreateInstance(backgroundFirework);
            backgroundFireworkEvent.start();
        }
        

        StartCoroutine(ReadyProcess());
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(remainGameTime);
            stream.SendNext(isPlaying);
            stream.SendNext(playerEnter);
        }
        else
        {
            this.remainGameTime = (float)stream.ReceiveNext();
            this.isPlaying = (bool)stream.ReceiveNext();
            this.playerEnter = (int[])stream.ReceiveNext();
        }
    }


    public override void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("플레이어 접속:" + player.NickName);

        if (PhotonNetwork.isMasterClient)
        {
            CreateOtherPlayerCharacter(player);
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("플레이어 접속 종료:" + otherPlayer.NickName);

        if (PhotonNetwork.isMasterClient)
        {
            for(int i=0; i<playerEnter.Length; i++)
            {
                if(playerEnter[i] == otherPlayer.ID)
                {
                    playerEnter[i] = -1;
                    playerReady[i] = false;
                    break;
                }
            }
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("플레이어 퇴장:" + PhotonNetwork.player.NickName);
        
    }

    public Vector3 GetPlayerRegenPos(int type)
    {
        if(type == 0)
        {
            bool isExist = false;
            bool isClose = false;

            Vector2 genPos = Vector2.zero;
            Vector2 objPos = Vector2.zero;

            for (int i=0; i<playerGenPos.Length; i++)
            {
                if (!Physics.Raycast(playerGenPos[i].position + Vector3.up, Vector3.down, 1.2f, LayerMask.GetMask("Ground")))
                {
                    continue;
                }

                Collider[]  overlapped = Physics.OverlapSphere(playerGenPos[i].position, 3.0f, LayerMask.GetMask("DynamicObject"));

                genPos.x = playerGenPos[i].position.x;
                genPos.y = playerGenPos[i].position.z;

                isExist = false;
                isClose = false;

                for (int j = 0; j < overlapped.Length; j++)
                {
                    if (overlapped[j].CompareTag("Player"))
                    {
                        isExist = true;
                        break;
                    }

                    objPos.x = overlapped[j].transform.position.x;
                    objPos.y = overlapped[j].transform.position.z;

                    if ((objPos - genPos).sqrMagnitude < 0.5f * 0.5f)
                    {
                        isClose = true;
                        break;
                    }
                }
                if (isExist || isClose)
                {
                    continue;
                }

                return playerGenPos[i].position;
            }
        }
        
        return itemManager.GetRegenPos(0);
    }

    private IEnumerator ReadyProcess()
    {
        bool localPlayerReady = false;
        
        
        while (!isPlaying)
        {
            if (Input.GetButtonDown("Ready") && !UIManager._instance.UIControl)
            {
                localPlayerReady = !localPlayerReady;
                if (localPlayerReady)
                {
                    GetPlayerByOwnerId(PhotonNetwork.player.ID).PC.ChangeState(PlayerAniState.Ready);
                    FMODUnity.RuntimeManager.PlayOneShot(readySound);
                }
                else
                {
                    GetPlayerByOwnerId(PhotonNetwork.player.ID).PC.ChangeState(PlayerAniState.Idle);
                    FMODUnity.RuntimeManager.PlayOneShot(unreadySound);
                }
                GetPlayerByOwnerId(PhotonNetwork.player.ID).IsControlable = !localPlayerReady;
                ServerManager.Send(string.Format("READY:{0}:{1}:{2}:{3}", InstanceValue.Nickname, InstanceValue.ID, InstanceValue.Room, localPlayerReady));
                photonView.RPC("SetPlayerReady", PhotonTargets.All, PhotonNetwork.player.ID, localPlayerReady);
            }
            yield return null;
        }
    }

    [PunRPC]
    private void SetPlayerReady(int ownerId, bool isReady)
    {
        PlayerStat player = GetPlayerByOwnerId(ownerId);
        player.isReady = isReady;
        player.PC.isUnbeatable = isReady;

        // ready UI 처리
        player.onReadyChanged(isReady);

        if (PhotonNetwork.isMasterClient)
        {
            bool allReady = true;

            for (int i = 0; i < playerEnter.Length; i++)
            {
                if (playerEnter[i] == ownerId)
                {
                    playerReady[i] = isReady;
                }
            }

            // 모든 플레이어가 레디 했는지 체크
            for(int i=0; i<playerReady.Length; i++)
            {
                if (!playerReady[i])
                {
                    allReady = false;
                    break;
                }
            }
            if(allReady)
            {
                // 자동시작
                GameStartRequest();
            }

            for(int i=0;i < playerReady.Length; i++)
            {       
                Debug.Log(i + ":" + playerEnter[i] + ", " + playerReady[i]);
            }
        }
    }

    /// <summary>
    /// 다른 사람의 플레이어블 캐릭터를 생성합니다. (마스터 전용)
    /// </summary>
    private void CreateOtherPlayerCharacter(PhotonPlayer player)
    {
        int characterIndex = -1;
        for (int i = 0; i < playerEnter.Length; i++)
        {
            if (playerEnter[i] == -1)
            {
                characterIndex = i;
                // ID작업
                playerEnter[i] = player.ID;
                break;
            }
        }

        if (characterIndex == -1)
        {
            Debug.LogError("남은 캐릭터 슬롯이 없음!");
            return;
        }

        photonView.RPC("RemoteCreatePlayer", player, characterIndex);
    }

    /// <summary>
    /// 플레이어 캐릭터 원격 생성 명령
    /// </summary>
    /// <param name="characterIndex">플레이어</param>
    [PunRPC]
    private void RemoteCreatePlayer(int characterIndex)
    {
        CreatePlayer(characterIndex, playerGenPos[characterIndex].position);
    }

    /// <summary>
    /// 플레이어를 생성합니다.
    /// </summary>
    public void CreatePlayer(int characterIndex, Vector3 spawnPosition)
    {
        StringBuilder prefabName = new StringBuilder("Prefabs/Character/");

        switch (characterIndex)
        {
            case 0:
                prefabName.Append("Dahong_0521");
                break;
            case 1:
                prefabName.Append("MingMing_0516");
                break;
            case 2:
                prefabName.Append("ShoSho_0605");
                break;
            case 3:
                prefabName.Append("CuChen_0612");
                break;
            default:
                prefabName.Append("Dahong_0424");
                break;
        }

        PhotonNetwork.Instantiate(prefabName.ToString(), spawnPosition, Quaternion.identity, 0);
        
        Debug.Log("플레이어 생성");
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
        isPlaying = true;
        ServerManager.Send(string.Format("START:{0}:{1}", InstanceValue.Room, isPlaying));
        photonView.RPC("RunGameEvent", PhotonTargets.AllBuffered, (int)GameEvent.GameStart);
        tempStartButton.SetActive(false);
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
                    itemManager.GameStartRegen();
                }
                SetMyPlayerActive(true);
                SetScoreLock(false);

                break;
            case GameState.Result:
                if (PhotonNetwork.isMasterClient)
                {
                    MapManager._instance.photonView.RPC("StopMapFacilities", PhotonTargets.All);
                    itemManager.active = false;

                    int focusedPlayerOwnerId = GetHighScorePlayerOwnerId();
                    photonView.RPC("Spotlight", PhotonTargets.All, focusedPlayerOwnerId);
                }
                isPlaying = false;
                SetMyPlayerActive(false);
                SetScoreLock(true);
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        StateManager.ChangeState((int)newState);
        currentState = newState;
        EnterState(newState);
    }

    /// <summary>
    /// 게임 이벤트 호출
    /// </summary>
    /// <param name="eventNum">호출할 게임 이벤트</param>
    [PunRPC]
    public void RunGameEvent(int eventNum)
    {
        switch(eventNum)
        {
            case (int)GameEvent.GameStart:
                if(PhotonNetwork.isMasterClient)
                    PhotonNetwork.room.IsOpen = false;

                SetPlayerUnbeatable(false);
                // 모든 플레이어의 레디 UI 비활성화
                for (int i = 0; i < playerList.Count; i++)
                {
                    playerList[i].onReadyChanged(false);
                    playerList[i].isReady = false;
                }
                UIManager._instance.readyInfo.SetActive(false);

                PlayerController myPlayer = GetPlayerByOwnerId(PhotonNetwork.player.ID).PC;
                UIManager._instance.respawnUI.Init();
                myPlayer.GameStartInit();
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
        if (isStart)
        {
            ChangeState(GameState.Playing);
            UIManager._instance.counterAnim.SetInteger("Count", 4);
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            ChangeState(GameState.Result);
        }
        UIManager._instance.counterAnim.gameObject.SetActive(false);
    }

    private void SetScoreLock(bool isLock)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].scoreLock = isLock;
        }
    }

    private void SetMyPlayerActive(bool active)
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

    private void SetPlayerUnbeatable(bool unbeatable)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].PC.isUnbeatable = unbeatable;
        }
    }

    /// <summary>
    /// 게임 진행 루프 (마스터만 사용)
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameLoop()
    {
        while (remainGameTime > 0)
        {
            if (timeProgress)
            {
                remainGameTime -= Time.deltaTime;

                if (!isStopping && remainGameTime <= 4)
                {
                    isStopping = true;
                    photonView.RPC("RunGameEvent", PhotonTargets.All, (int)GameEvent.GameStop);
                }
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

    /// <summary>
    /// 특정 플레이어를 카메라로 확대해 보여줍니다
    /// </summary>
    /// <param name="spotlightOwnerId">보여줄 플레이어의 OwnerId</param>
    [PunRPC]
    private void Spotlight(int spotlightOwnerId)
    {
        ServerManager.Send(string.Format("WIN:{0}:{1}:{2}", InstanceValue.Nickname, InstanceValue.ID, InstanceValue.Score));
        PlayerController focusedPlayer = GetPlayerByOwnerId(spotlightOwnerId).GetComponent<PlayerController>();

        // 플레이어 업데이트
        focusedPlayer.Spotlight();

        // 카메라 업데이트
        cameraController.ChangeMode(CameraMode.FrontView);
        cameraController.SetTarget(focusedPlayer.transform);

        // UI 업데이트
        UIManager._instance.ChangeScreen(UIManager.ScreenType.Result);
        UIManager._instance.resultScoreBoard.CalcResult();
        UIManager._instance.resultEmotion.SetPlayerController(focusedPlayer);
        if(focusedPlayer.photonView.isMine)
            UIManager._instance.resultEmotion.gameObject.SetActive(true);


        // 시야를 방해하는 오브젝트들 끄기
        Collider[] colls = Physics.OverlapBox(focusedPlayer.transform.position + new Vector3(0, 4.0f, -2f), new Vector3(1.0f, 4.0f, 2.0f), Quaternion.identity, LayerMask.GetMask("DynamicObject", "SubObject", "TableTopObject"));
        for(int i=0; i<colls.Length; i++)
        {
            if (colls[i].CompareTag("Player")) continue;

            colls[i].gameObject.SetActive(false);
        }
        itemManager.StopAllFeature();


        // 배경의 폭죽 켜기
        for (int i = 0; i < backgroundFireworks.Length; i++)
        {
            backgroundFireworks[i].SetActive(true);
        }

        // 배경음악 재생
        SoundManager._instance.SetBGM(resultBGM);

        // 캐릭터 승리대사 재생
        focusedPlayer.PublicPlayVoiceSound("Victory");
    }

    public void StartLoadTitleScene()
    {
        if(!isSceneMoving)
        {
            if(Connected._instance != null)
                Destroy(Connected._instance.gameObject);
            PhotonNetwork.player.SetScore(0);
            PhotonNetwork.Disconnect();
            StartCoroutine(LoadTitleScene());
            isSceneMoving = true;

            if (backgroundFireworkSoundEnable)
            {
                backgroundFireworkEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        else
        {
            Debug.LogWarning("이미 작동 중 입니다.");
        }
    }

    private IEnumerator LoadTitleScene()
    {
        // 타이틀씬을 완벽하게 로딩 후 씬을 변경한다
        AsyncOperation oper = SceneManager.LoadSceneAsync(0);

        yield return oper; // 로딩이 완료될때까지 대기 한다
    }
}
