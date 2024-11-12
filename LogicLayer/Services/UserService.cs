using LogicLayer.IRepos;
using LogicLayer.Entitys;

namespace LogicLayer.Classes
{
    public class UserService
    {
        private readonly IUserRepo repository;

        public UserService(IUserRepo userRepo)
        {
            repository = userRepo;
        }

        public void CreateUserIfNotExisting(User user)
        {
            if (!repository.UserExists(user.id))
            {
                repository.CreateUser(user);
            }
            else
            {
                Console.WriteLine("User already exists, no action taken.");
            }
        }
    }
}
