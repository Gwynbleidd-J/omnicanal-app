using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LoginForms.Models;

namespace LoginForms
{
    static class GlobalSocket
    {
        private static Socket socket;
        private static User user;

        public static Socket GlobalVarible
        {
            get { return socket; }
            set { socket = value; }
        }

        public static User currentUser
        {
            get { return user; }
            set { user = value; }
        }
        public static string numberToSend { get; set; }
    }
}
