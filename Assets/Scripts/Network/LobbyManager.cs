using UnityEngine;
using System;
using System.IO;

public class LobbyManager : InstanceValue
{
    private static LobbyManager _lobby;
    public static LobbyManager Lobby
    {
        get { return _lobby; }
        set { _lobby = value; }
    }
    public GameObject PhotonLobby;

    void Start()
    {
        PhotonLobby.SetActive(false);
    }
}