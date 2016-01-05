using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace temp_network.Scripts
{
    class Protocol
    {
        static JsonSerializer serializer = new JsonSerializer();

        public bool StartToClient(NetworkMessage stringMsg, int herenumber_, System.IO.StreamWriter OtherWriter, System.IO.StreamWriter SoW)
        {
            try
            {
                serializer.Serialize(OtherWriter, stringMsg);
                OtherWriter.Flush();
                Console.WriteLine("Room" + herenumber_.ToString() + " 's Load Message Send Successed");
            }
            catch
            {
                Console.WriteLine("Room" + herenumber_.ToString() + " 's Load Message Send Failed");
                return false;
            }
            try
            {
                serializer.Serialize(SoW, stringMsg);
                SoW.Flush();
                Console.WriteLine("Room" + herenumber_.ToString() + " 's Load Message Send Successed");
            }
            catch
            {
                Console.WriteLine("Room" + herenumber_.ToString() + " 's Load Message Send Failed");
                return false;
            }
            return true;
        }

        public bool ConnectToClient(NetworkMessage stringMsg, int herenumber_, System.IO.StreamWriter OtherWriter, System.IO.StreamWriter SoW)
        {
            try
            {
                serializer.Serialize(OtherWriter, stringMsg);
                OtherWriter.Flush();
                Console.WriteLine("Room" + herenumber_.ToString() + " 's Connect Message Send Successed");
            }
            catch
            {
                Console.WriteLine("Room" + herenumber_.ToString() + " 's Connect Message Send Failed");
                return false;
            }
            try
            {
                serializer.Serialize(SoW, stringMsg);
                SoW.Flush();
                Console.WriteLine("Room" + herenumber_.ToString() + " 's Connect Message Send Successed");
            }
            catch
            {
                Console.WriteLine("Room" + herenumber_.ToString() + " 's Connect Message Send Failed");
                return false;
            }

            return true;
        }

        public bool MoveStartToClient(NetworkMessage stringMsg, int herenumber_, System.IO.StreamWriter OtherWriter)
        {
            try
            {
                serializer.Serialize(OtherWriter, stringMsg);
                OtherWriter.Flush();
                Console.WriteLine("Room" + herenumber_.ToString() + " 's MoveStart Message Send Successed");
                return true;
            }
            catch
            {
                Console.WriteLine("Room" + herenumber_.ToString() + " 's MoveStart Message Send Failed");
                return false;
            }
        }

        public bool MoveEndToClient(NetworkMessage stringMsg, int herenumber_, System.IO.StreamWriter OtherWriter)
        {
            try
            {
                serializer.Serialize(OtherWriter, stringMsg);
                OtherWriter.Flush();
                Console.WriteLine("Room" + herenumber_.ToString() + " 's MoveEnd Message Send Successed");
                return true;
            }
            catch
            {
                Console.WriteLine("Room" + herenumber_.ToString() + " 's MoveEnd Message Send Failed");
                return false;
            }
        }
    }
}
