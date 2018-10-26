﻿using ServerModule;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonConnection : Photon.PunBehaviour {

    private void Start()
    {
        ConnectToPhoton();
    }

    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 4;

        Hashtable table = new Hashtable();

        if (SceneDataManager._instance.isSelect)
        {
            table["MapNum"] = SceneDataManager._instance.mapNum;
        }
        else
        {
            table["MapNum"] = Random.Range(0, (int)GameMap.Max);
        }


        roomOptions.CustomRoomProperties = table;

        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        // 닉네임 설정
        PhotonNetwork.playerName =  InstanceValue.Nickname;
        ServerManager.Send(string.Format("NICKNAME:{0}:{1}", InstanceValue.Nickname, InstanceValue.ID));
        PhotonNetwork.JoinRandomRoom();
    }

    // 방 생성 성공 이벤트(Photon 호출)
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("OnCreatedRoom");
    }

    // 방 접속 성공 이벤트 (Photon 호출)
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        if(PhotonNetwork.room != null)
        {
            Hashtable table = PhotonNetwork.room.CustomProperties;
            GameMap map = (GameMap)table["MapNum"];
            Debug.Log("map: " + map);

            PhotonNetwork.isMessageQueueRunning = false;

            StartCoroutine(LoadGameScene(map));
        }
        else
        {
            Debug.LogError("룸 정보를 가져올 수 없습니다.");
        }

        Debug.Log("OnJoinedRoom");
    }

    public void ConnectToPhoton()
    {
        // 게임 버전
        PhotonNetwork.ConnectUsingSettings("0.8.6");
    }

    IEnumerator LoadGameScene(GameMap map)
    {
        // 게임씬을 완벽하게 로딩 후 씬을 변경한다
        AsyncOperation oper = SceneManager.LoadSceneAsync((int)map+2);

        yield return oper; // 로딩이 완료될때까지 대기 한다
    }
    
}
