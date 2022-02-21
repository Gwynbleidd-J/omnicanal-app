using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LoginForms.Models;
using SocketIOClient;

namespace LoginForms
{
    static class GlobalSocket
    {
        private static SocketIO socket;
        private static User user;

        public static SocketIO GlobalVarible
        {
            get { return socket; }
            set { socket = value; }
        }

        public static User currentUser
        {
            get { return user; }
            set { user = value; }
        }

        public static Message message { get; set; }

        public static string numberToSend { get; set; }
    }
}
