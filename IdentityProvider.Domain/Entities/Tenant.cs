using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityProvider.Domain.Entities
{
    public class Tenant : Entity
    {
        
        public string Name { get; private set; }
        public string ConnectionString { get; private set; }


        public void CreateConnectionString()
        {
            ConnectionString = $"teste{Name}";
        }
    }
}
