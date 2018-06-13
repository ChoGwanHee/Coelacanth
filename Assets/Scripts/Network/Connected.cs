using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerModule;
using System;
using System.Threading;

public class Connected : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("초기화");
        ServerManager.Initialized();
    }

    private void Start()
    {
        StartConnectServer("127.0.0.1", 2020);
    }

    private void StartConnectServer(string _address, int _port)
    {
        Debug.Log("서버 접속 시도");
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
            ServerManager.Send("DISCONNECT");
            Thread.Sleep(500);
            InstanceValue.TCP.Close();
        }
        StopCoroutine(PacketProc());
    }
}