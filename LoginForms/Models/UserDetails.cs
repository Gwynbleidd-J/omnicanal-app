using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms.Models
{
    public class UserDetails
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string paternalSurname { get; set; }
        public string maternalSurname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string agentIdentifierWhatsapp { get; set; }
        public string agentIdentifierTelegram { get; set; }
        public string activeChats { get; set; }
        public string activeIp { get; set; }
        public string rolID { get; set; }
        public string leaderId { get; set; }
        public string token { get; set; }
        public Rol rol { get; set; }
        public UserStatus status { get; set; }
    }
}
