using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityProvider.Domain.Handler
{
    public interface IUserHandler
    {
        bool ValidateCredentials(string email, string password);
    }
}
