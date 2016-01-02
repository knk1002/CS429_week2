using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;

public class NetworkClient {

    public System.IO.StreamWriter streamWriter;
    public System.IO.StreamReader streamReader;
    public TcpClient Client_Socket;
    public NetworkStream netStream;

    public bool init()
    {
        try
        {
            Client_Socket = new TcpClient("143.248.36.223", 2345);
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

    public void SendtoServer()
    {

    }
}
