using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManagerPhoton : Photon.PunBehaviour
{

    public static GameManagerPhoton _instance;

    public Transform[] playerGenPos = null;
    public float gameTick = 0.1f;

    public Texture2D cursorTex;

    public CameraController cameraController;

    public ItemManager itemManager;

    [FMODUnity.EventRef]
    public string BGM;



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


        CreatePlayer();

        FMODUnity.RuntimeManager.PlayOneShot(BGM);
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

        // 나간 유저 UI 빼기
        UIManager._instance.DeActiveOtherStatus((int)playerProperties["PlayerIndex"]);

        cameraController.FindPlayers();
        Debug.Log("플레이어 접속 종료:" + otherPlayer.NickName);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("플레이어 퇴장:" + PhotonNetwork.player.NickName);
    }

    // 플레이어 생성
    public void CreatePlayer()
    {
        Hashtable customProperties = PhotonNetwork.player.CustomProperties;
        int playerIndex = (int)customProperties["PlayerIndex"];

        Vector3 pos = playerGenPos[playerIndex].position;
        string prefabName;

        switch (playerIndex)
        {
            case 0:
                prefabName = "Dahong_0424";
                break;
            case 1:
                prefabName = "Chorok_0426";
                break;
            case 2:
                prefabName = "Blue_0426";
                break;
            case 3:
                prefabName = "Juhwang_0426";
                break;
            default:
                prefabName = "Dahong_0424";
                break;
        }

        PhotonNetwork.Instantiate("Prefabs/Character/"+prefabName, pos, Quaternion.identity, 0);

        Debug.Log("플레이어 생성");

        // 자기 UI 초기화
        UIManager._instance.UIInitialize(playerIndex);

    }


    public void RespawnPlayer(Transform playerTF)
    {
        Hashtable playerProperties = PhotonNetwork.player.CustomProperties;
        Vector3 pos = playerGenPos[(int)playerProperties["PlayerIndex"]].position;
        pos.y += 0.5f;

        playerTF.position = pos;
    }
    
    

}
