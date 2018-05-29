using UnityEngine;
using System;
using System.IO;

public class LobbyManager : InstanceValue
{
    public GameObject PhotonLobby;

    private RoomManager _room;

    void Start()
    {
        PhotonLobby.SetActive(false);
        _room = new RoomManager();
    }
}