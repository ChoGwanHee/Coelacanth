using System;

// http://treeofimaginary.tistory.com/96 .NET exception 버그 참고
// Unity\Editor\Data\PlaybackEngines\windowsstandalonesupport\Variations\win32_development_mono\Data\Managed
public class ServerManager : InstanceValue
{
	public static InstanceValue Instance = new InstanceValue();
	private static ServerManager _server;
	public static ServerManager Server
	{
		get { return _server; }
		set { _server = value; }
	}

	private void Initialized()
	{
		// server argument
		Instance.TCP = null;
		Instance.Address = null;
		Instance.Port = 0;
		Instance.BufferSize = new byte[4096];
		Instance.Receive = 0;
		// lobby argument
		Instance.Connected = false;
		Instance.State = (int)PeerValue.Disconnected;
		Instance.ID = 0;
		Instance.Version = null;
		Instance.Nickname = null;
	}
	public virtual void Connect(string _address, int _port) { Initialized(); }
	public virtual void Disconnect() { }
	public virtual void StringSplitsWordsDelimiter(string _text) { }
	public virtual void ParsePacket(int _length) { }
	public virtual void Send(string _text) { }
	public static void WriteLine(string _text) { Console.WriteLine(_text); }
	public byte[] ShortToByte(int _value)
	{
		byte[] temp = new byte[2];
		temp[1] = (byte)((_value & 0x0000ff00) >> 8);
		temp[0] = (byte)((_value & 0x000000ff));
		return temp;
	}
}