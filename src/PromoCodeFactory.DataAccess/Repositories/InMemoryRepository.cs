﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        private IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var item = Data.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return Task.FromResult(false);
            }

            Data = Data.Where(x => x.Id != id);
            return Task.FromResult(true);
        }

        public Task<Guid> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            Data = Data.Append(entity);
            return Task.FromResult(entity.Id);
        }

        public Task<bool> UpdateByIdAsync(Guid id, T entity)
        {
            var item = Data.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return Task.FromResult(false);
            }

            Data = Data.Select(x => x.Id == id ? entity : x);
            return Task.FromResult(true);
        }
    }
}