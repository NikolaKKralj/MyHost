using MyHostAPI.Data.Interfaces;
using MyHostAPI.Data.Specifications;
using Quartz;
using Quartz.Spi;

namespace MyHostAPI.SceduledServices.Jobs
{
    public class RemoveUnconfirmedUsersJob : IJob
    {
        private readonly IUserRepository _userRepository;

        public RemoveUnconfirmedUsersJob(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var users = await _userRepository.FindManyByAsync(new ActiveUsers());

            var usersToDelete = users.Where(x => x.Identity.EmailConfirmed == false && x.CreatedOn < DateTime.Now.AddDays(-1));

            foreach (var user in usersToDelete)
            {
                await _userRepository.RemoveAsync(user.Id);
            }
        }
    }
}
