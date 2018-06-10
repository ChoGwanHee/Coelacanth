using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerModule;
using System;

public class testServer : MonoBehaviour
{

    //ServerModule.ServerManager _server = new ServerModule.ServerManager();
    
    //private void Start()
    //{
    //    try
    //    {            
    //        _server.Connect("127.0.0.1", 2020);
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Log(ex.ToString());
    //        throw;
    //    }
    //    Debug.Log("Connected To Server");
        
    //    //StartCoroutine(PacketProc());
    //}

    //IEnumerator PacketProc()
    //{
    //    while (true)
    //    {
    //        if (Instance.TCP.Available > 0)
    //        {
    //            byte[] _buffer = new byte[4096];
    //            int _read = Instance.TCP.Receive(_buffer, Instance.TCP.Available, 0);
    //            if (_read > 0)
    //            {
    //                Buffer.BlockCopy(_buffer, 0, Instance.BufferSize, Instance.Receive, _read);
    //                Instance.Receive += _read;
    //                while (true)
    //                {
    //                    int _length = BitConverter.ToInt16(Instance.BufferSize, 0);
    //                    if (_length > 0 && Instance.Receive >= _length)
    //                    {
    //                        _server.ParsePacket(_length);
    //                        Instance.Receive -= _length;
    //                        Buffer.BlockCopy(Instance.BufferSize, _length, Instance.BufferSize, 0, Instance.Receive);
    //                    }
    //                    else
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        yield return null;
    //    }
    //}
} 