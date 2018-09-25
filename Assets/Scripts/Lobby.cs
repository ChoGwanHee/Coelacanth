using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : Photon.PunBehaviour {

    private void Start()
    {
        // 게임 버전
        PhotonNetwork.ConnectUsingSettings("0.7.1");
    }

    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    public override void OnJoinedLobby()
    {
        // 닉네임 설정
        PhotonNetwork.playerName = "TestPlayer " + Random.Range(0, 20000);

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

        StartCoroutine(LoadGameScene());

        Debug.Log("OnJoinedRoom");
    }

    IEnumerator LoadGameScene()
    {
        // 게임씬을 완벽하게 로딩 후 씬을 변경한다
        AsyncOperation oper = SceneManager.LoadSceneAsync(2);

        yield return oper; // 로딩이 완료될때까지 대기 한다
    }
}
