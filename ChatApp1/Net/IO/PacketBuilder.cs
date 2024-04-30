using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    class PacketBuilder //all this class is going to do is allow us to append a data to a memory stream 
    {
        MemoryStream _ms; //importing a memorystream and calling it _ms
        public PacketBuilder() //create a constructor 
        {
            _ms = new MemoryStream();

        }
        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }
        public void WriteMessage(string msg) //core function to be used to send messages too not only username 
        {
            var msgLength = msg.Length; //reading the length of the message 
            _ms.Write(BitConverter.GetBytes(msgLength)); //this will be read as opcode length with how many bytes we actually need to get that string.
            _ms.Write(Encoding.ASCII.GetBytes(msg)); //samething for msg too. if op code is 1byte  4byte for message string we sending is hello There. that is 11 characters that will be stored as 4 bytes
        } 
        
        //How many Op code is the maximum the server can get using 1 byte?

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray(); //this is all our packet builder as a whole // get the memory stream as whole byte 
        }
    }
}
