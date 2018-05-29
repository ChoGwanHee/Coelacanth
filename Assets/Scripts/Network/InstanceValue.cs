using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public enum PeerValue
{
	Disconnected,
	Connected,
	Disconnecting,
	Connecting,
	RoomIn,
	RoomOut,
	Ready,
	Win,
	Lose
}

public class InstanceValue : MonoBehaviour
{
	// server argument
	private Socket _tcp = null;
	public Socket TCP
	{
		get { return _tcp; }
		set { _tcp = value; }
	}

	private string _address;
	public string Address
	{
		get { return _address; }
		set { _address = value; }
	}

	private int _port;
	public int Port
	{
		get { return _port; }
		set { _port = value; }
	}

	private byte[] _buffersize;
	public byte[] BufferSize
	{
		get { return _buffersize; }
		set { _buffersize = value; }
	}

	private int _receive;
	public int Receive
	{
		get { return _receive; }
		set { _receive = value; }
	}

	// lobby argument
	private bool _connected;
	public bool Connected
	{
		get { return _connected; }
		set { _connected = value; }
	}

	private int _id;
	public int ID
	{
		get { return _id; }
		set { _id = value; }
	}

	private int _state;
	public int State
	{
		get { return _state; }
		set { _state = value; }
	}

	private string _version;
	public string Version
	{
		get { return _version; }
		set { _version = value; }
	}

	private string _nickname;
	public string Nickname
	{
		get { return _nickname; }
		set { _nickname = value; }
	}
}
