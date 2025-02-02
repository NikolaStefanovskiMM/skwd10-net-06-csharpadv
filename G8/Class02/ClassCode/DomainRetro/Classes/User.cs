﻿using DomainRetro.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainRetro.Classes
{
    public abstract class User : IUser
    {
        public int Id { get; set; }
        public string Username { get; set; }    
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public abstract void PrintUser();
    }
}
