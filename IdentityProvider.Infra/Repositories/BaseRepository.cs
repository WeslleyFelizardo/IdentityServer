using IdentityProvider.Domain.Entities;
using IdentityProvider.Domain.Repositories;
using IdentityProvider.Infra.Datas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace IdentityProvider.Infra.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        private readonly IdentityContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(IdentityContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetByExpression(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public T GetById(int id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public T Save(T entity)
        {
            _dbSet.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);

            _context.SaveChanges();

            return entity;
        }
    }
}
