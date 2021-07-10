using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms.Models
{
    public class SoftphoneCredentials
    {
        public bool requiredRegister { get; set; }
        public string displayName { get; set; }
        public string userName { get; set; }
        public string registerName { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
        public int port { get; set; }
        public string proxy { get; set; }

    }
}
