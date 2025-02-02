﻿using Class13.TaxiManager9000.Domain.Entities;

namespace Class13.TaxiManager9000.DataAccess.Interfaces
{
    public interface IDb<T> where T : BaseEntity
    {
        int Add(T entity);
        bool RemoveById(int id);
        bool Update(T entity);
        List<T> GetAll();
        T GetById(int id);
    }
}
