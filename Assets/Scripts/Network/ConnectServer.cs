using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;
using FMOD;
using UnityEngine;
using Debug = FMOD.Debug;

public class ConnectServer : ServerManager
{
    public ConnectServer(string _address, int _port)
    {
        try
        {
            Connect(_address, _port);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        StartCoroutine(PacketProc());
    }

    public override void Connect(string _address, int _port)
    {
        base.Connect(_address, _port);
        IPAddress serverIP = IPAddress.Parse(_address);
        int serverPort = Convert.ToInt32(_port);
        Instance.TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Instance.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 3000);
        Instance.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
        Instance.TCP.Connect(new IPEndPoint(serverIP, serverPort));
        Console.WriteLine("Server Connect To Client (" + serverIP + ":" + serverPort + ")");
        LobbyManager.Lobby.PhotonLobby.SetActive(true);
    }

    IEnumerator PacketProc()
    {
        while (true)
        {
            if (Instance.TCP.Available > 0)
            {
                byte[] _buffer = new byte[4096];
                int _read = Instance.TCP.Receive(_buffer, Instance.TCP.Available, 0);
                if (_read > 0)
                {
                    Buffer.BlockCopy(_buffer, 0, Instance.BufferSize, Instance.Receive, _read);
                    Instance.Receive += _read;
                    while (true)
                    {
                        int _length = BitConverter.ToInt16(Instance.BufferSize, 0);
                        if (_length > 0 && Instance.Receive >= _length)
                        {
                            ParsePacket(_length);
                            Instance.Receive -= _length;
                            Buffer.BlockCopy(Instance.BufferSize, _length, Instance.BufferSize, 0, Instance.Receive);
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

    public override void StringSplitsWordsDelimiter(string _text)
    {
        // UPDT_DATE : 2018-05-29
        // UPDT_NAME : 조관희
        // REMK_TEXT : 정규표현식 Split 패킷 분할 작업
        base.StringSplitsWordsDelimiter(_text);
        char[] delimiterChars = { ':' };
        string text = _text;
        string[] words = text.Split(delimiterChars);
        Console.WriteLine(string.Format("{0} words in text", words.Length));
        foreach (string word in words)
        {
            Console.WriteLine(word);
        }
    }

    public override void ParsePacket(int _length)
    {
        base.ParsePacket(_length);
        string message = Encoding.UTF8.GetString(Instance.BufferSize, 2, _length - 2);
        string[] text = message.Split(':');

        switch(text[0])
        {
            case "CONNECT":
                Send(string.Format("CONNECT"));
                Console.WriteLine("Client Server Connected : " + text[0]);
                break;
            case "DISCONNECT":
                Console.WriteLine("Client Server Disconnected : " + text[0]);
                break;
            default:
                break;
        }
    }

    public override void Send(string _text)
    {
        base.Send(_text);
        try
        {
            if (Instance.TCP != null && Instance.TCP.Connected)
            {
                byte[] _buffer = new byte[4096];
                Buffer.BlockCopy(ShortToByte(Encoding.UTF8.GetBytes(_text).Length + 2), 0, _buffer, 0, 2);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(_text), 0, _buffer, 2, Encoding.UTF8.GetBytes(_text).Length);
                Instance.TCP.Send(_buffer, Encoding.UTF8.GetBytes(_text).Length + 2, 0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
    
    public override void Disconnect()
    {
        base.Disconnect();
        StopCoroutine(PacketProc());
    }

    void OnDestroy()
    {
        if (Instance.TCP != null && Instance.TCP.Connected)
        {
            Send(string.Format("DISCONNECT"));
            Thread.Sleep(500);
            Instance.TCP.Close();
        }
    }
}