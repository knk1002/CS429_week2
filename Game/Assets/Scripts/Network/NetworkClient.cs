using UnityEngine;
using System.Net.Sockets;
using Newtonsoft.Json;

public class NetworkClient {

    public System.IO.StreamWriter streamWriter;
    public System.IO.StreamReader streamReader;
    public TcpClient Client_Socket;
    public NetworkStream netStream;

    public bool init()
    {
        try
        {
            Client_Socket = new TcpClient("143.248.233.58", 2345);
            //Client_Socket = new TcpClient("127.0.0.1", 2345);
        }
        catch
        {
            Debug.Log("Failed to connect to server");
            return false;
        }

        netStream = Client_Socket.GetStream();
        streamWriter = new System.IO.StreamWriter(netStream);
        streamReader = new System.IO.StreamReader(netStream);

        Debug.Log("Connected to Server");

        return true;
    }

    public void SendtoServer(NetworkMessage message)
    {
        JsonSerializer serializer = new JsonSerializer();
        try
        {
            serializer.Serialize(streamWriter, message);
            streamWriter.Flush();
        }
        catch
        {
            Debug.Log("Message Send Failed");
        }
    }

	public void SendLoad(int networkOrder)
	{
		SendtoServer(new NetworkMessage(NetworkMessage.MessageType.Load, new System.Object[0], networkOrder));
	}

	public void SendMoveStart(string dir, Vector3 pos, int networkOrder)
	{
		SendtoServer(new NetworkMessage(NetworkMessage.MessageType.MoveStart, new System.Object[4] {dir, pos.x, pos.y, pos.z}, networkOrder));
	}

	public void SendMoveEnd(int networkOrder, Vector3 pos)
	{
		SendtoServer(new NetworkMessage(NetworkMessage.MessageType.MoveEnd, new System.Object[3] { pos.x, pos.y, pos.z }, networkOrder));
	}

    public void SendBallColide(int networkOrder, Vector3 pos, Vector2 velo)
    {
        SendtoServer(new NetworkMessage(NetworkMessage.MessageType.BallColide, new System.Object[5] { pos.x, pos.y, pos.z ,velo.x,velo.y}, networkOrder));
    }

    public void SendDie(int networkOrder)
    {
        SendtoServer(new NetworkMessage(NetworkMessage.MessageType.Die, new System.Object[0], networkOrder));
    }

    public void SendBrickColide(int networkOrder, int num)
    {
        SendtoServer(new NetworkMessage(NetworkMessage.MessageType.BrickColide, new System.Object[1] { num }, networkOrder));
    }
}
