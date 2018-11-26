using IdentityProvider.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IdentityProvider.Domain.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        T Save(T entity);

        T Update(T entity);

        T GetById(int id);

        IEnumerable<T> GetAll();

        void Delete(T entity);

        T GetByExpression(Expression<Func<T, bool>> expression);
    }
}
