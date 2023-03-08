using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public static class SocketExtensions
    {
        public static byte[] ReceiveAll(this Socket socket)
        {
            var buffer = new List<byte>();

            while (socket.Available > 0)
            {
                var currByte = new Byte[1];
                try
                {
                    var byteCounter = socket.Receive(currByte, currByte.Length, SocketFlags.None);

                    if (byteCounter == 1)
                    {
                        buffer.Add(currByte[0]);
                    }
                }  
                catch { continue; }
            }

            return buffer.ToArray();
        }
    }
}
