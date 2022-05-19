﻿namespace TaxiManager9000.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public BaseEntity()
        {
            Id = -1;
        }

        public BaseEntity(int id)
        {
            Id = id;
        }
    }
}
 