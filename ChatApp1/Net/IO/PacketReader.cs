using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;


namespace ChatClient.Net.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns) //creating a constructor that takes NetworkStream and passing it in the base class
        {
            _ns = ns;
        }

        public string ReadMessage() // in here we will have just one function to read the message
        {
            byte[] msgBuffer;
            var length = ReadInt32();//temporary buffer
            msgBuffer = new byte[length];
            _ns.Read(msgBuffer, 0, length);//temporary buffer
                                           //reading the msgbuffer from the zero lenght 

            var msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
             
            //read the payload off the packet 
        }
    }
}
