using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;

public class ServerManager : MonoBehaviour
{    
    // Socket TCP 변수 선언
    private Socket _tcp = null;
    public Socket TCP
    {
        get { return _tcp; }
        set { _tcp = value; }
    }

    // 서버 인터넷 프로토콜 주소
    private string _address;
    public string Address
    {
        get { return _address; }
        set { _address = value; }
    }

    // 서버 프로토콜 포트 값
    private int _port;
    public int Port
    {
        get { return _port; }
        set { _port = value; }
    }

    // 서버 데이터의 버퍼
    private byte[] _buffer;
    public byte[] BufferSize
    {
        get { return _buffer; }
        set { _buffer = value; }
    }

    // 받은 통신 데이터의 길이
    private int _receive;
    public int Receive
    {
        get { return _receive; }
        set { _receive = value; }
    }

    // property initialize
    private void initialize()
    {
        // TCP 서버 초기화
        TCP = null;
        Address = "192.168.0.10";
        Port = 2020;
        BufferSize = new byte[4096];
        Receive = 0;
    }

    private void Start()
    {
        StartConnectServer();
    }

    public virtual void StartConnectServer() { initialize(); Debug.Log("TCP 초기화"); }    // 서버 접속 시도
    public virtual void DisconnectServer() // 서버 접속 종료
    {
        // 서버 접속 상태를 해제한다.
        if (TCP != null && TCP.Connected)
        {
            TCP.Close();
        }
    }
    public virtual void ParsePacket(int length)
    {
        // 1. 실제 데이터의 문자열을 가져온다.
        // 2. 이벤트 관리를 위해 ':' 문자로 규약시킨다.
        // 3. 수신 된 패킷의 이벤트 문자열을 대조한다.
        // 4. 클라이언트에서 서버 통신이 가능한 경우, 허용 메시지 패킷을 보낸다.
        // 5. 클라이언트 접속이 종료되면, 종료 메시지 패킷을 보낸다.
    }
    public void SendServerMassage(string _text)
    {
        // 서버에서 처리하고자 하는 이벤트 형식의 문자열을 보내주는 함수이다.
        // 접속, 끊기, 시작, 준비, 이동 등 
        try
        {
            // 1. 서버 연결상태 확인
            // 2. 임시버퍼 정의
            // 3. Buffer.BlockCopy 메서드 (Array, Int32, Array, Int32, Int32) : 2바이트 문자열 길이로 표현한다.
            //    https://msdn.microsoft.com/ko-kr/library/system.buffer.blockcopy.aspx
            // 4. 보내는 문자열 데이터를 byte 형변환해서 버퍼에 넣는다.
            // 5. 2Byte 크기의 데이터와 실질적인 문자열 데이터로 구성하여 서버로 송신한다.

            if (TCP != null && TCP.Connected)
            {
                byte[] _buffer = new byte[4096];
                Buffer.BlockCopy(ShortToByte(Encoding.UTF8.GetBytes(_text).Length + 2), 0, _buffer, 0, 2);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(_text), 0, _buffer, 2, Encoding.UTF8.GetBytes(_text).Length);
                TCP.Send(_buffer, Encoding.UTF8.GetBytes(_text).Length + 2, 0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
    public byte[] ShortToByte(int val)
    {
        // 1. 다량의 패킷을 분산시켜주기 위해 int형 변수를 받아온다.
        // 2. int -> byte 형변환
        byte[] temp = new byte[2];
        temp[1] = (byte)((val & 0x0000ff00) >> 8);
        temp[0] = (byte)((val & 0x000000ff));
        return temp;
    }
    public IEnumerator PacketProc() // 서버로부터 들어오는 패킷 처리 함수
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
                    Debug.Log("Receive Data : " + Receive);
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
}