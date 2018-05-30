using UnityEngine;
using System;
using System.IO;

public class LobbyManager : InstanceValue
{
    public GameObject PhotonLobby;

    private static LobbyManager _lobby;
    public static LobbyManager Lobby
    {
        get { return _lobby; }
        set { _lobby = value; }
    }

    void Start()
    {
        PhotonLobby.SetActive(false);
        Lobby = GetComponent<LobbyManager>();
        Debug.Log("Lobby Object Disabled");
    }

    public void AvailablePhoton()
    {
        PhotonLobby.SetActive(true);
        Debug.Log("Lobby object Availabled");
    }
}