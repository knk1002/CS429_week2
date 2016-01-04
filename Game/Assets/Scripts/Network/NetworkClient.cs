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
            //Client_Socket = new TcpClient("143.248.36.223", 2345);
            Client_Socket = new TcpClient("127.0.0.1", 2345);
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

	public void SendMoveStart(string dir, int networkOrder)
	{
		SendtoServer(new NetworkMessage(NetworkMessage.MessageType.MoveStart, new System.Object[1] {dir}, networkOrder));
	}

	public void SendMoveEnd(int networkOrder)
	{
		SendtoServer(new NetworkMessage(NetworkMessage.MessageType.MoveEnd, new System.Object[0], networkOrder));
	}
}
