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

    public override string ToString()
    {
        try
        {
            string res = Type.ToString() + ": /";
            if(Arguments != null)
            {
                for (int i = 0; i < Arguments.Length; i++)
                {
                    res += Arguments[i].ToString() + "/";
                }
            }            
            res += NetworkOrder.ToString();
            return res;
        }
        catch
        {
            Console.WriteLine("Json ToString Failed");
            return "";
        }

    }
}
