using Bus.Commands;
using IdentityProvider.Domain.Entities;
using IdentityProvider.Domain.Repositories;
using MassTransit;
using System.Threading.Tasks;

namespace IdentityProvider.Infra.EventBus.Consumers
{
    public class CreateUserConsumer : IConsumer<CreatedEmployeeCommandEvent>
    {
        private readonly IRepository<User> _repositoryUser;
        public CreateUserConsumer(IRepository<User> repositoryUser)
        {
            _repositoryUser = repositoryUser;
        }
        public async Task Consume(ConsumeContext<CreatedEmployeeCommandEvent> context)
        {
            var message = context.Message;

            var user = new User(message.Email, message.Name, message.Password, null);
            user.PermissionEmployee();

            _repositoryUser.Save(user);
            await Task.CompletedTask;
        }
    }
}
