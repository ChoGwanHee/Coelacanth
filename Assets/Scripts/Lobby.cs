using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Lobby : Photon.PunBehaviour {

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.3.7");
    }

    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 4;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "PlayerEnter0", false }, { "PlayerEnter1", false }, { "PlayerEnter2", false }, { "PlayerEnter3", false } };
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    public override void OnJoinedLobby()
    {
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
        PhotonNetwork.isMessageQueueRunning = false;

        // 닉네임
        PhotonNetwork.playerName = "TestPlayer " + Random.Range(0, 20000);

        // 플레이어 커스텀 프로퍼티 조정
        Hashtable roomProperties = PhotonNetwork.room.CustomProperties;
        int playerIndex = 0;

        for(int i=0; i<4; i++)
        {
            if(!(bool)roomProperties["PlayerEnter"+i])
            {
                playerIndex = i;
                break;
            }
        }
        Hashtable playerProperties = new Hashtable() { { "PlayerIndex", playerIndex } };
        PhotonNetwork.player.SetCustomProperties(playerProperties);

        roomProperties["PlayerEnter" + playerIndex] = true;
        PhotonNetwork.room.SetCustomProperties(roomProperties);

        StartCoroutine(LoadGameScene());

        Debug.Log("OnJoinedRoom");
    }

    IEnumerator LoadGameScene()
    {
        // 게임씬을 완벽하게 로딩 후 씬을 변경한다
        AsyncOperation oper = SceneManager.LoadSceneAsync("twinvilla");
        //AsyncOperation oper = SceneManager.LoadSceneAsync("boomboomparty");
        yield return oper; // 로딩이 완료될때까지 대기 한다
    }
}
