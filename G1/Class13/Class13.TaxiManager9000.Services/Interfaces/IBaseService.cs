﻿using Class13.TaxiManager9000.Domain.Entities;

namespace Class13.TaxiManager9000.Services.Interfaces
{
    public interface IBaseService<T> where T : BaseEntity
    {
        bool Add(T user);
        bool Remove(int id);
        T GetById(int id);
        Task<List<T>> GetAll();
        List<T> GetAll(Func<T, bool> wherePredicate);
    }
}
