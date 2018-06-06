using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerModule;

public class testServer : MonoBehaviour
{
    ServerModule.ConnectServer _server;
    private void Start()
    {
        Debug.Log("Used .dll");
        _server = new ServerModule.ConnectServer();
        Debug.Log("END .dll");
    }
}