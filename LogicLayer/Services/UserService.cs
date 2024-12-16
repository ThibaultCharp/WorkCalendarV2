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

        public List<User> GetAllUsersWithoutEmployer(string Input)
        {
            return repository.GetAllEmployeesWithoutEmployer(Input);
        }

        public List<User> GetAllUsersWithCorrespondingRole(string Input) 
        {
            return repository.GetAllUsersWithCorrespondingRoles(Input);
        }

        public void LinkUser (string LoggedInUserEmail, string TargetUserEmail)
        {
            repository.LinkUser(LoggedInUserEmail, TargetUserEmail);
        }

        public void ChangeUserRole(string email, int userId, bool makeEmployer)
        {
            if(userId == 2)
            {
                makeEmployer = true;
            }
            repository.ChangeUserRole(email, userId, makeEmployer);
        }
    }
}
