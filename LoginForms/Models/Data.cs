﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms.Models
{
    public class Data
    {
        public List<Message> messages { get; set; }
        public List<Chat> chats { get; set; }

        public List<Permission> permissions { get; set; }

        public List<Menu> menus { get; set; }

        public List<User> users { get; set; }

        public List<NetworkCategories> networks { get; set; }

        public List<UserStatus> status { get; set; }

        public List<UserDetails> details { get; set; }

        public User user { get; set; }

        //public UserStatus userStatus { get; set; }
    }
}
