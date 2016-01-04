using System;

[Serializable]
public class NetworkMessage
{
    public enum MessageType
    {
		PlayerOrder,
        Connect,
		Load,
		Start,
		MoveStart,
		MoveEnd
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
