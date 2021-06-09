using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms.Models
{
    public class Permission
    {
       public int id { get; set; }
       public string name { get; set; }
        public string description { get; set; }
        public string rolId { get; set; }
        public string menuId { get; set; }
        public string create { get; set; }
        public string read { get; set; }
        public string update { get; set; }
        public string delete { get; set; }

        public Menu menu { get; set; }

    }
}
