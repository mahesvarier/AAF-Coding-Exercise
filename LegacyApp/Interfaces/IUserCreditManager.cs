using LegacyApp.Models;

namespace LegacyApp.Interfaces
{
    public interface IUserCreditManager
    {
        void SetUserCreditLimit(User user, Client client);
    }
}
