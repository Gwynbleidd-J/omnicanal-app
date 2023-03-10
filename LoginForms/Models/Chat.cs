using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms.Models
{
    public class Chat
    {
        public string id { get; set; }
        public string startTime { get; set; }
        public string endingTime { get; set; }
        public string score { get; set; }
        public string comments { get; set; }
        public string file { get; set; }
        public string userId { get; set; }
        public string networkCategoryId { get; set; }
        public string clientPlatformIdentifier { get; set; }
        public string clientPhoneNumber { get; set; }
        public string date { get; set; }
        public string platformIdentifier { get; set; }
    }
}
