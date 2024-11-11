using LegacyApp.Interfaces;
using LegacyApp.Models;

namespace LegacyApp.Imeplenentations
{
    public class UserDataAccessWrapper : IUserDataAccessWrapper
    {
        public void AddUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}
