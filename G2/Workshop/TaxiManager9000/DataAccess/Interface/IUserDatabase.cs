﻿using TaxiManager9000.Domain.Entities;

namespace TaxiManager9000.DataAccess.Interface
{
    public interface IUserDatabase : IFileDatabase<User>
    {
        User GetByUserNameAndPassword(string username, string password);
        
        User GetByUserName(string userName);
    }
}
