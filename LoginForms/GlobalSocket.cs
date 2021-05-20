using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace LoginForms
{
    static class GlobalSocket
    {
        private static Socket socket;

        public static Socket GlobalVarible
        {
            get { return socket; }
            set { socket = value; }
        }
    }
}
