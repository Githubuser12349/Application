using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System;
using ChatServer.Net.IO;


namespace ChatServer
{
    class Program
    {
        static List<Client> _users;

        static TcpListener _listener; //Creating a tcplistner to listen to any upcoming connecting coming for the Client 
        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("192.168.128.74"), 9000); //creatting a new instance of listner
            _listener.Start(); //we could add a backlog too for better error handling 

            //Adding as much client as possible in this while loop

            while (true) //creating multiple clients 
            {
                var client = new Client(_listener.AcceptTcpClient()); //creating a client
                _users.Add(client);

                BroadcastConnection();

            }
            /*Broadcast the connection to everyone on the server*/






        }
        static void BroadcastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.Username); //what does this 
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());

                }
            }
        }

        public static void BroadcastMessage(string message) //Broadcasting the messages to every client
        {
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string uid) //Creating a disconnect packet 
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUser);

            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());

            }

            BroadcastMessage($"[{disconnectedUser.Username}] : Disconnected");
        }
    }
}
