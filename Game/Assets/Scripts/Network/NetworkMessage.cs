using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class NetworkMessage
{
    public enum MessageType
    {
        Connect
    }

    public int NetworkOrder;
    public MessageType Type;
    public System.Object[] Arguments;

    public NetworkMessage(MessageType type, System.Object[] arguments, int networkOrder = -1)
    {
        Type = type;
        NetworkOrder = networkOrder;
        Arguments = arguments;
    }
}
