using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class LobbyManager : MonoBehaviour
{
    public GameObject PhotonLobby, NetworkManager;

    private static LobbyManager _lobby;
    public static LobbyManager Lobby
    {
        get { return _lobby; }
        set { _lobby = value; }
    }

    void Start()
    {
        PhotonLobby.SetActive(false);
        NetworkManager.SetActive(false);
        Lobby = GetComponent<LobbyManager>();
        //Debug.Log("Lobby Object Disabled");
    }

    public void AvailablePhoton()
    {
        //Debug.Log("Lobby object Availabled");
        NetworkManager.SetActive(true);
        Thread.Sleep(1000);
        PhotonLobby.SetActive(true);
        PhotonLobby.GetComponent<Lobby>().ConnectToPhoton();
    }
}