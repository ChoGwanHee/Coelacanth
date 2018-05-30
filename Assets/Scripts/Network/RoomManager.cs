using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using Random = System.Random;

public class RoomManager
{
    readonly static int RoomMaxCount = 2;
    
    private string _roomid;
    public string RoomId
    {
        get { return _roomid; }
        set { _roomid = value; }
    }

    private int _maxplyer;
    public int MaxPlayer
    {
        get { return _maxplyer; }
        set { _maxplyer = value; }
    }

    private bool _ready;
    public bool Ready
    {
        get { return _ready; }
        set { _ready = value; }
    }

    public RoomManager()
    {
        RoomId = null;
        MaxPlayer = 0;
        Ready = false;
        RoomInitialize();
    }
    
    // 룸이 생성될 때, 작동하는 함수
    // 최대인원 4명을 수용할 수 있는 환경을 조성한다.
    private void RoomInitialize()
    {
        // PK값 생성
        Random _random = new Random();
        RoomId = PrivateCharKey(_random, 20);
        Debug.Log("HashValue : " + RoomId);
    }

    // 룸의 정보를 구분할 수 있는 PK 값 생성하는 함수
    private string PrivateCharKey(Random _random, int _length)
    {
        string charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        return PrivateCharKey(_random, _length, charPool);
    }
     
    private string PrivateCharKey(Random _random, int _length, string _pool)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _length; i++)
            sb.Append(_pool[(int) (_random.NextDouble() * _pool.Length)]);
        return sb.ToString();
    }
}