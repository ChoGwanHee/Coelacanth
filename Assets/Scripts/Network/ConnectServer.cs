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

    static Vector3 position, velocity;
    static Quaternion rotate;
    private bool _isMove;
    public bool ISMOVE
    {
        get { return _isMove; }
        set { _isMove = value; }
    }



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
        // 5. 서버 접속 수신시간 제한 (30초)MovePacketReceive
        // 6. 서버 접속 시도

        IPAddress serverIP = IPAddress.Parse(addr);
        int serverPort = Convert.ToInt32(port);
        TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
        TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
        TCP.Connect(new IPEndPoint(serverIP, serverPort));
        Debug.Log("Server Connect (" + serverIP + ":" + serverPort + ")");
    }

    // 서버로부터 들어오는 패킷 처리 함수
    IEnumerator PacketProc()
    {
        // 1. 서버에서 받은 데이터가 있으면, 수신 된 데이터의 길이를 확인한다.
        // 2. 임시 데이터를 받아 버퍼에 데이터를 쌓는다.
        // 3. 검증 데이터에 필요한 2바이트 데이터를 저장한다.
        while (true)
        {
            // TODO : DISCONNECT 과정에서 패킷이 넘어오지 않음 -> NullReferenceException
            if (TCP.Available > 0) // 소켓에서 들어온 데이터가 있으면 SocketTCP.Available은 수신받은 데이터 길이를 나타낸다.
            {
                byte[] buff = new byte[4096]; // 데이터를 받기 위한 임시 버퍼
                int nread = TCP.Receive(buff, TCP.Available, 0); // 소켓이 수신한 데이터를 buff로 읽어온다.
                if (nread > 0) // Receive 함수는 실제로 데이터를 받은 길이를 리턴한다. 이 값이 0 이상이면 실제로 뭔가를 받아온 상태이다.
                {
                    Buffer.BlockCopy(buff, 0, BufferSize, Receive, nread); // 방금 받아온 데이터를 buffer에 누적시킨다.
                    Receive += nread; // 실제로 받아온 데이터 길이를 증감시킨다.
                    // 여기서 중요한 점은 특정 행동에 대한 패킷을 버퍼에 뭉쳐놓는다.
                    // 얻고자 하는 실제 데이터와 맞지 않으면, 패킷이 나눠져서 보내지고 처리되기 때문에 레이턴시가 늦춰질 수 있다.
                    while (true)
                    {
                        // 수신 된 2바이트 데이터를 저장한다.
                        int length = BitConverter.ToInt16(BufferSize, 0);

                        if (length > 0 && Receive >= length)
                        {
                            // 1. 받은 데이터의 길이 + 2바이트보다 큰 패킷의 검증여부를 진행한다.
                            // 2. 패킷 파싱
                            // 3. 연산이 끝나면 패킷데이터 관리를 위해 1개의 길이를 감소시킨다.
                            // 4. 감소 된 데이터 길이 만큼 버퍼를 제거한다.
                            ParsePacket(length);
                            Receive -= length;
                            Buffer.BlockCopy(BufferSize, length, BufferSize, 0, Receive);
                        }
                        else
                        {
                            // 1개 이상의 패킷을 처리해야되기 때문에 패킷이 없으면 빠져나가서 대기한다.
                            break;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    // 클래스에서 보낸 메시지를 파싱해서 처리하는 함수
    public void MovePacketReceive(string _message, Vector3 _position, Quaternion _rotate, Vector3 _velocity)
    {
        if (_message.Equals("MOVE"))
        {
            position = _position;
            rotate = _rotate;
            velocity = _velocity;
        }
    }

    public float _time = 0.2f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SendServerMassage(string.Format("GOOD"));
        }
        if (_time > 0.0f)
        {
            _time -= Time.deltaTime;
        }
        else
        {
            SendServerMassage(string.Format("PLAYER:{0}:{1}:{2}:{3}", position, rotate, velocity, _sequance));
            _time = 0.2f;
        }

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
            SendServerMassage(string.Format("GAMESTART"));
        }
        else if (text[0].Equals("PLAYER"))
        {
            Debug.Log(msg);
        }
        else if (text[0].Equals("GOOD"))
        {
            Debug.Log(msg);
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