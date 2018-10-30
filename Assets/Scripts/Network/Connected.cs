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
        string ip = ConfigManager.ReadValue(0);

        if(ip == null)
        {
            // default IP
            //ip = "45.112.165.82";
            ip = "192.168.0.10";
        }
        StartConnectServer(ip, 12800);
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
            Thread.Sleep(500);
            InstanceValue.TCP.Close();
        }
        StopCoroutine(PacketProc());
        Thread.Sleep(100);
        Destroy(this);
    }
}