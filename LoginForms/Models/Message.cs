using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms.Models
{
    public class Message
    {
        public string id { get; set; }
        public string messagePlatformId { get; set; }
        public string time { get; set; }
        public string text { get; set; }
        public string transmitter { get; set; }
        public int statusId { get; set; }
        public string chatId { get; set; }
        public string platformIdentifier { get; set; }
        public string clientPlatformIdentifier { get; set; }


           // "platformIdentifier": 'w', "clientPlatformIdentifier: '24243244323422'"
    }

}
