using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    public float gameTick = 0.1f;
    
    /// <summary>
    /// 플레이어가 생성되는 위치 배열
    /// </summary>
    public Transform[] playerGenPos = null;

    /// <summary>
    /// 커서 이미지
    /// </summary>
    public Texture2D cursorTex;

    [HideInInspector]
    public CameraController cameraController;
    [HideInInspector]
    public ItemManager itemManager;

    [FMODUnity.EventRef]
    public string BGM;

    /// <summary>
    /// 현재 존재하는 플레이어의 리스트
    /// </summary>
    [HideInInspector]
    public List<PlayerStat> playerList = new List<PlayerStat>();


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

        Cursor.SetCursor(cursorTex, new Vector2(43, 43), CursorMode.ForceSoftware);

        PhotonNetwork.isMessageQueueRunning = true;

        Hashtable customProperties = PhotonNetwork.player.CustomProperties;
        int playerIndex = (int)customProperties["PlayerIndex"];
        CreatePlayer(playerIndex, playerGenPos[playerIndex].position);

        //FMODUnity.RuntimeManager.PlayOneShot(BGM);
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
                prefabName = "ShoSho_0530";
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

}
