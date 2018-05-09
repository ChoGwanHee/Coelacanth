using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;
using UnityEngine.UI;

public class ConnectServer : ServerManager
{
    // (노동요) : https://www.youtube.com/watch?v=FJfPp8OU0Ak&list=UUw8ZhLPdQ0u_Y-TLKd61hGA
    [HideInInspector]
    public Transform _player;
    [HideInInspector]
    public string _sequance;
    Vector3 MovePosition;

    // 클라이언트의 서버 접속을 위한 함수
    public override void StartConnectServer()
    {
        base.StartConnectServer();
        Debug.Log("서버 접속 시도");
        // 서버 연결중이면, 끊어주고 시작한다.
        DisconnectServer();
        try
        {   // 서버 접속 시도
            serverInitialize(Address, Port);
            //multicastServer(UDPaddress, Port);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
        // 서버로부터 받아들이는 패킷 데이터 처리
        StartCoroutine(PacketProc());
    }

    // TCP 클래스의 서버 주소 등록 함수
    private void serverInitialize(string addr, int port)
    {
        // 1. 서버 아이피 어드레스 설정
        // 2. 서버 포트값 설정
        // 3. TCP 소켓 생성
        // 4. 서버 접속 송신시간 제한 (30초)
        // 5. 서버 접속 수신시간 제한 (30초)
        // 6. 서버 접속 시도

        IPAddress serverIP = IPAddress.Parse(addr);
        int serverPort = Convert.ToInt32(port);
        TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
        TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
        TCP.Connect(new IPEndPoint(serverIP, serverPort));
        Debug.Log("Server Connect (" + serverIP + ":" + serverPort + ")");
    }

    // 서버에서 보낸 패킷을 파싱해서 처리하는 함수
    public override void ParsePacket(int length)
    {
        string msg = Encoding.UTF8.GetString(BufferSize, 2, length - 2);
        string[] text = msg.Split(':');
        if (text[0].Equals("CONNECT"))
        {
            Debug.Log("Client Server Connected : " + text[0]);
            SendServerMassage(string.Format("CONNECT:{0}:{1}", Address, Port));
            //StringSplitsWordsDelimiter("abc"); // 패킷 구분처리 테스
        }
        else if (text[0].Equals("DISCONNECT"))
        {
            Debug.Log("Client Server Disconnected : " + text[0]);
            SendServerMassage(string.Format("DISCONNECT:{0}", _sequance));
        }
        else if (text[0].Equals("INITIALIZE"))
        {
            Debug.Log("Play to game set data : " + text[0]);
            _sequance = text[1];
            SendServerMassage(string.Format("GAMESTART:{0}", MovePosition));
        }
        else if (text[0].Equals("POSITION"))
        {
            Debug.Log(msg);
            //SendServerMassage(string.Format("POSITION:{0}:{1}:{2}:{3}", position, rotate, velocity, _sequance));
        }
    }

    void OnDestroy()
    {
        // 1. 서버 접속 상태 여부를 확인한다.
        // 2. 연결 종료 패킷을 보낸다.
        if (TCP != null && TCP.Connected)
        {
            SendServerMassage("DISCONNECT"); // 서버로 DISCONNECT 패킷을 보내 접속을 끊어준다.
            Thread.Sleep(500); // 서버로 패킷이 온전히 도착할때까지 기다려준다.
            TCP.Close(); // 소켓을 닫는다.
        }
        StopCoroutine(PacketProc()); // 코루틴을 중지시킨다.
    }

    public override void DisconnectServer()
    {
        base.DisconnectServer();
        StopCoroutine(PacketProc());
    }
}