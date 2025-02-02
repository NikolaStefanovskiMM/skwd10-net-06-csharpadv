﻿using Class13.TaxiManager9000.Domain.Enums;

namespace Class13.TaxiManager9000.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public User()
        { }

        public User(string username, string password, Role role)
        {
            Username = username;
            Password = password;
            Role = role;
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override string Print()
        {
            return $"User {Id} - {Username} and role {Role}";
        }
    }
}
