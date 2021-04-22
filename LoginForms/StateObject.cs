using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms
{
    class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int bufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[bufferSize];
        // Received data string.  
        public StringBuilder stringBuilder = new StringBuilder();
    }
}
