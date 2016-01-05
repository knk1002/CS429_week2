using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace temp_network.Scripts
{
    static class NetworkServer
    {
        static TcpListener Server_Listener = new TcpListener(IPAddress.Any, 2345);
        static List<int> SocketNumList = new List<int>();
        static List<Socket[]> WholeSocketList = new List<Socket[]>();
        static List<Boolean[]> WholeConnectList = new List<Boolean[]>();
        static List<Boolean[]> WholeLoadList = new List<Boolean[]>();
        static List<Socket> SocketList = new List<Socket>();
        static System.Object[] RndObject = new System.Object[100];
        static int RoomNumber = -1;
        static string RandNumString = "";

        static void Listener(Socket Serv_Socket)
        {

            Socket Server_Socket = Serv_Socket;
            Server_Socket.ReceiveTimeout = 1500;

            int herenumber = 0;
            int playernumber = 0;
            DateTime timer = DateTime.Now;
            bool connected = false;
            bool connectsend = false;
            bool issend = false;
            bool iscorrectmsg = false;
            Protocol _protocol = new Protocol();
            Console.WriteLine("Client:" + Server_Socket.RemoteEndPoint + "now connected to server");

            NetworkStream netStream = new NetworkStream(Server_Socket);
            System.IO.StreamWriter streamOwnWriter = new System.IO.StreamWriter(netStream);
            System.IO.StreamReader streamOwnReader = new System.IO.StreamReader(netStream);
            NetworkStream netOtherStream = null;
            System.IO.StreamWriter streamOtherWriter = null;

            if (RoomNumber == -1)
            {
                Socket[] TmpSocketArray = new Socket[2];
                Boolean[] TmpBooleanArray = new Boolean[2] { false, false };
                Boolean[] TmpBooleanArray2 = new Boolean[2] { false, false };
                WholeSocketList.Add(TmpSocketArray);
                WholeConnectList.Add(TmpBooleanArray);
                WholeLoadList.Add(TmpBooleanArray2);
                RoomNumber += 1;
            }

            herenumber = RoomNumber;
            int[] PNRN = new int[2];
            playernumber = PlayerOrdertoClient(Serv_Socket, streamOwnWriter, herenumber, RandNumString, RndObject);

            while (true)
            {
                NetworkMessage msgData = null;
                JsonSerializer serializer = new JsonSerializer();
                iscorrectmsg = true;
                
                if(netStream.DataAvailable)
                {
                    try
                    {
                        msgData = serializer.Deserialize<NetworkMessage>(new JsonTextReader(streamOwnReader));
                    }
                    catch
                    {
                        Console.WriteLine("Not Json Form Messeage");
                        iscorrectmsg = false;
                    }
                }
                    
                bool OpponentSignal = DisconnectOtherPlayer(herenumber, playernumber, connected);
                if (OpponentSignal)
                    break;

                if(!connectsend && WholeConnectList[herenumber][1])
                {
                    if (playernumber == 1)
                    {
                        netOtherStream = new NetworkStream(WholeSocketList[herenumber][1]);
                        streamOtherWriter = new System.IO.StreamWriter(netOtherStream);
                    }
                    else
                    {
                        netOtherStream = new NetworkStream(WholeSocketList[herenumber][0]);
                        streamOtherWriter = new System.IO.StreamWriter(netOtherStream);
                    }

                    if (playernumber == 2)
                        _protocol.ConnectToClient(new NetworkMessage(NetworkMessage.MessageType.Connect, null), herenumber, streamOtherWriter, streamOwnWriter);

                    connectsend = true;
                }                   


                if (iscorrectmsg && msgData != null && (playernumber != 0))
                {
                    Console.WriteLine(msgData.ToString());

                    if (WholeConnectList[herenumber][1])
                    {
                        if (!connected)
                        {
                            
                            connected = true;
                        }

                        issend = false;
                        if(msgData.Type == NetworkMessage.MessageType.Load)
                        {
                            WholeLoadList[herenumber][playernumber] = true;
                            issend = true;
                            if (WholeLoadList[herenumber][0] && WholeLoadList[herenumber][1])
                                issend = _protocol.StartToClient(new NetworkMessage(NetworkMessage.MessageType.Start, null),herenumber,streamOtherWriter,streamOwnWriter);
                        }
                        else
                            issend = MessageClassify(msgData, herenumber, streamOwnWriter, streamOtherWriter, _protocol);
                    }
                    else
                    {
                        Console.WriteLine("Opponent Not Connected");
                    }

                }

                System.Threading.Thread.Sleep(1);
            }

            netStream.Close();
            if(netOtherStream != null)
                netOtherStream.Close();
            if(streamOtherWriter != null)
                streamOtherWriter.Close();
            streamOwnReader.Close();
            streamOwnWriter.Close();

            try
            {
                WholeConnectList[herenumber][playernumber - 1] = false;
                WholeSocketList[herenumber][playernumber - 1] = null;
            }
            catch
            {
                Console.WriteLine("playernumber Index Error Or Herenumber Index Error");
            }

            Console.WriteLine("End");
            
            
        }

        static int PlayerOrdertoClient(Socket Serv_Socket, System.IO.StreamWriter SoW, int herenumber_, string RandNumString, System.Object[] RndObject_)
        {
            bool isSend = true;
            JsonSerializer serializer = new JsonSerializer();

            if (!WholeConnectList[RoomNumber][0])
            {
                WholeConnectList[RoomNumber][0] = true;
                WholeSocketList[RoomNumber][0] = Serv_Socket;
                try
                {
                    System.Object[] temp_Object = new System.Object[] { 1 };
                    NetworkMessage temp_message = new NetworkMessage(NetworkMessage.MessageType.PlayerOrder, temp_Object, -1);
                    serializer.Serialize(SoW, temp_message);
                    SoW.Flush();
                    Console.WriteLine("Room " + herenumber_.ToString() + " Player1 Order Message Send Successed");
                }
                catch
                {
                    Console.WriteLine("Room " + herenumber_.ToString() + " Player1 Order Message Send Failed");
                    isSend = false;
                }

                if (!isSend)
                {
                    WholeConnectList[RoomNumber][0] = false;
                    WholeSocketList[RoomNumber][0] = null;
                    return 0;
                }
                return 1;
            }
            else
            {
                isSend = true;
                WholeConnectList[RoomNumber][1] = true;
                WholeSocketList[RoomNumber][1] = Serv_Socket;

                try
                {
                    System.Object[] temp_Object = new System.Object[] { 2 };
                    NetworkMessage temp_message = new NetworkMessage(NetworkMessage.MessageType.PlayerOrder, temp_Object, -1);
                    serializer.Serialize(SoW, temp_message);
                    SoW.Flush();
                    Console.WriteLine("Room " + herenumber_.ToString() + " Player2 Order Message Send Successed");
                }
                catch
                {
                    Console.WriteLine("Room " + herenumber_.ToString() + " Player2 Order Message Send Failed");
                    isSend = false;
                }

                if (!isSend)
                {
                    WholeConnectList[RoomNumber][1] = false;
                    WholeSocketList[RoomNumber][1] = null;

                    return 0;
                }
                else
                {
                    RoomNumber += 1;
                    Socket[] TmpSocketArray = new Socket[2];
                    Boolean[] TmpBooleanArray = new Boolean[2] { false, false };
                    Boolean[] TmpBooleanArray2 = new Boolean[2] { false, false };
                    WholeSocketList.Add(TmpSocketArray);
                    WholeConnectList.Add(TmpBooleanArray);
                    WholeLoadList.Add(TmpBooleanArray2);
                }
            }
            return 2;
        }

        static System.Object[] PlayerOrderPlusRnd(System.Object[] InputObject, int PO)
        {
            System.Object[] ResultObject = new System.Object[101];

            for (int i = 0; i < 100; i++)
                ResultObject[i] = InputObject[i];

            ResultObject[100] = PO;

            return ResultObject;
        }

        static bool MessageClassify(NetworkMessage msgData_, int herenumber_, System.IO.StreamWriter SoW, System.IO.StreamWriter OtherWriter, Protocol _protocol)
        {
            switch (msgData_.Type)
            {
                case NetworkMessage.MessageType.MoveStart:
                    return _protocol.MoveStartToClient(msgData_, herenumber_, OtherWriter);
                case NetworkMessage.MessageType.MoveEnd:
                    return _protocol.MoveEndToClient(msgData_, herenumber_, OtherWriter);
                default:
                    Console.WriteLine("Unknown Case");
                    return false;
            }
        }

        static bool DisconnectOtherPlayer(int herenumber_, int playernumber_, bool connected_)
        {
            if (connected_)
            {
                if (playernumber_ == 1)
                {
                    if (!WholeConnectList[herenumber_][playernumber_])
                    {
                        Console.WriteLine("Room " + herenumber_.ToString() + " 's player" + playernumber_.ToString() + " also disconnect");
                        return true;
                    }

                }
                else if (playernumber_ == 2)
                {
                    if (!WholeConnectList[herenumber_][playernumber_ - 2])
                    {
                        Console.WriteLine("Room " + herenumber_.ToString() + " 's player" + playernumber_.ToString() + " also disconnect");
                        return true;
                    }
                }
            }

            return false;
        }


        public static void Main()
        {
            Server_Listener.Start();
            Console.WriteLine("Server Start");
            LoginAccept temp_a = new LoginAccept();
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                int temp_rnd = rnd.Next(1,100000);
                RndObject[i] = temp_rnd.ToString();
            }
                while (true)
                {
                    if (Server_Listener.Pending())
                    {
                        Socket Serv_Socket = Server_Listener.AcceptSocket();

                        if (Serv_Socket.Connected)
                        {
                            Thread newThread = new Thread(new ThreadStart(
                                delegate()
                                {
                                    Listener(Serv_Socket);
                                }));
                            newThread.Start();
                            Console.WriteLine("new Thread");
                        }
                    }

                    System.Threading.Thread.Sleep(1);
                }
        }
    }
}
