using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        public string Username { get; set; } //getting a username from the user 
        public Guid UID { get; set; }  //GUID stands for gloablly unique identifier, it can be used later on to specify with client 
                                       //we are interested in interacting with later on 

        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;
         
        public Client(TcpClient client) //adding a constructor 
        {
            ClientSocket = client;
            UID = Guid.NewGuid(); //generating a new userid whenever the new client is generated. 
            _packetReader = new PacketReader(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();



            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Username}"); //Modified in a way when the client joins it broadcast when did he/she join 

            Task.Run(() => Process()); //it will help with the thread and not interfer with other UID 

            //Have an overview about the threading error in c#
        }
        void Process()
        {
            while(true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch(opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}: [{Username}] sent:  {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            //calling the function broadcasting message
                            break;
                        default:
                            break;

                    }
                }
                catch(Exception)
                {
                    Console.WriteLine($"[{UID.ToString()}] [{Username}] : has succesfully disconnected!");
                    Program.BroadcastDisconnect(UID.ToString()); //broadcast the disconnection
                    ClientSocket.Close(); //Handling disconnection 
                    break;
                }
            }
        } 
    }
}

