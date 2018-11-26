using IdentityProvider.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityProvider.Domain.Entities
{
    public class User : Entity
    {
        public User(string email, string name, string password, int? tenantId)
        {
            Email = email;
            Name = name;
            Password = password;
            TenantId = tenantId;
        }

        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public int? TenantId { get; private set; }
        public Permissions Permission { get; private set; }

        public void PermissionEmployee()
        {
            Permission = Permissions.Employee;
        }

    }
}
