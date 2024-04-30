using ChatClient.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    class Server
    {
            TcpClient _client;
            public PacketReader PacketReader;
            PacketBuilder _packetBuilder;
            public event Action connectedEvent;
           public event Action msgRecievedEvent;
           public event Action userDisconnectEvent;

   
            public Server() //Start by creating a constructer
            {
                _client = new TcpClient();
            }
            public void ConnectToServer(string username) //calling the username from mainviewmodel 
            {
                if (!_client.Connected)
                {
                    _client.Connect("192.168.128.74", 9000); //creating a client interface that connects to this specified Server + Port 
                     PacketReader = new PacketReader(_client.GetStream()); //this is where we are going to start sending data.

                    if (!string.IsNullOrEmpty(username))
                    {
                         var connectPacket = new PacketBuilder();
                         connectPacket.WriteOpCode(0);
                         connectPacket.WriteMessage(username);
                         _client.Client.Send(connectPacket.GetPacketBytes()); //then by this function we will send the packet to the server 
                    }
                     ReadPackets();
                }
            }

               

        private void ReadPackets()
        {
            Task.Run(() =>
          {
              while(true)
              {
                  var opcode = PacketReader.ReadByte();
                  switch(opcode)
                  {
                      case 1:
                          connectedEvent?.Invoke();
                          break;

                     case 5:
                          msgRecievedEvent.Invoke();
                          break;

                      case 10:
                          userDisconnectEvent?.Invoke();
                          break; 


                      default:
                          Console.WriteLine("ah yes");
                          break;
                  }
              }
          });
        }

        public void SendMessageToServer(string message) //forming a constructor to be able to send messages
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());

        }
    }
}

