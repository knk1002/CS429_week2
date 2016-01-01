using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Assets.Scripts.Network
{
    [Serializable]
    public class NetworkMessage
    {
        public enum MessageType
        {
            Connect
        }

        public int NetworkOrder;
        public MessageType Type;
        public Object[] Arguments;

        public NetworkMessage(MessageType type, Object[] arguments, int networkOrder = -1)
        {
            Type = type;
            NetworkOrder = networkOrder;
            Arguments = arguments;
        }
    }
}
