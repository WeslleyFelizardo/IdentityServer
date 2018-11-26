using IdentityProvider.Domain.Entities;
using IdentityProvider.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityProvider.Domain.Handler
{
    public class UserHandler : IUserHandler
    {
        private readonly IRepository<User> _repositoryUser;
        public UserHandler(IRepository<User> repositoryUser)
        {
            _repositoryUser = repositoryUser;
        }
        public bool ValidateCredentials(string email, string password)
        {
            var user = _repositoryUser.GetByExpression(u => u.Email == email);

            if (password.Equals(user?.Password))
                return true;

            return false;
        }
    }
}
