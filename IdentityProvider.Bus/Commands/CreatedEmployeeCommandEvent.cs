using IdentityProvider.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bus.Commands
{
    public sealed class CreatedEmployeeCommandEvent : CommandEvent
    {
        public CreatedEmployeeCommandEvent(string email, string password, int idEmployee, string name) : base()
        {
            Email = email;
            Password = password;
            IdEmployee = idEmployee;
            Name = name;
        }

        public string Email { get; private set; }
        public string Password { get; private set; }
        public int IdEmployee { get; private set; }
        public string Name { get; private set; }
    }
}
