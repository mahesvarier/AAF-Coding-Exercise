using LegacyApp.Interfaces;
using LegacyApp.Models;
using System;
using System.Threading.Tasks;

namespace LegacyApp.Imeplenentations
{
    public class UserService : IUserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;
        private readonly IUserDataAccessWrapper _userDataAccessWrapper;

        public UserService(IClientRepository clientRepository, IUserCreditService userCreditService,
            IUserDataAccessWrapper userDataAccessWrapper)
        {
            _clientRepository = clientRepository;
            _userCreditService = userCreditService;
            _userDataAccessWrapper = userDataAccessWrapper;
    }

        public async Task<User> AddUser(string firstName, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            UserValidator.ValidateUserInput(firstName, surname, email, dateOfBirth);

            var client = await _clientRepository.GetByIdAsync(clientId);
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstName,
                Surname = surname
            };

            UserCreditManager.SetUserCreditLimit(user, client, _userCreditService);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                throw new InvalidOperationException("Insufficient credit limit.");
            }

            _userDataAccessWrapper.AddUser(user);

            return user;
        }
    }
}
