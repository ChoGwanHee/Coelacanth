using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerModule;
using System;
using System.Threading;

public class Connected : MonoBehaviour
{
    public static Connected _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(this);
            ServerManager.Initialized();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        // Service
        // server_ip:45.112.165.82
        // Fork
        // server_ip:192.168.0.10
        // Local
        // server_ip:127.0.0.1
        string ip;
        string stringPort;
        int port;
        ConfigManager.ReadValue("server_ip", out ip);
        ConfigManager.ReadValue("server_port", out stringPort);

        if (!Int32.TryParse(stringPort, out port))
        {
            port = 12800;
            Debug.LogWarning("Port 정보를 불러올 수 없어 기본 Port로 설정되었습니다.");
        }

        if (ip == null)
        {
            // default IP
            ip = "45.112.165.82";
            Debug.LogWarning("IP 정보를 불러올 수 없어 기본 IP로 설정되었습니다.");
        }
        StartConnectServer(ip, port);
        //StartConnectServer("192.168.0.15", 12800);
        
    }

    void NicknameWriteLine()
    {
        Debug.Log(InstanceValue.Nickname);
    }

    private void StartConnectServer(string _address, int _port)
    {
        ServerManager.Disconnect();

        try
        {
            ServerManager.TCPConnect(_address, _port);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            throw;
        }
        StartCoroutine(PacketProc());
    }

    IEnumerator PacketProc()
    {
        while (true)
        {
            if (InstanceValue.TCP.Available > 0)
            {
                byte[] _buffer = new byte[4096];
                int _read = InstanceValue.TCP.Receive(_buffer, InstanceValue.TCP.Available, 0);
                if (_read > 0)
                {
                    Buffer.BlockCopy(_buffer, 0, InstanceValue.BufferSize, InstanceValue.Receive, _read);
                    InstanceValue.Receive += _read;
                    while (true)
                    {
                        int _length = BitConverter.ToInt16(InstanceValue.BufferSize, 0);
                        if (_length > 0 && InstanceValue.Receive >= _length)
                        {
                            ServerManager.ParsePacket(_length);
                            InstanceValue.Receive -= _length;
                            Buffer.BlockCopy(InstanceValue.BufferSize, _length, InstanceValue.BufferSize, 0, InstanceValue.Receive);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (InstanceValue.TCP != null && InstanceValue.TCP.Connected)
        {
            ServerManager.Send(string.Format("DISCONNECT:{0}:{1}:{2}", InstanceValue.Nickname, InstanceValue.ID, InstanceValue.Room));
            Thread.Sleep(100);
            InstanceValue.TCP.Close();
        }
        StopCoroutine(PacketProc());
        Thread.Sleep(100);
        Destroy(this);
    }
}