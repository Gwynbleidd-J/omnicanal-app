using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms.Models
{
    public class AppParameters
    {
        public string twilioAccountSID { get; set; }
        public string twilioAuthToken { get; set; }
        public string whatsappAccount { get; set; }
        public string botTokenTelegram { get; set; }
    }
}
