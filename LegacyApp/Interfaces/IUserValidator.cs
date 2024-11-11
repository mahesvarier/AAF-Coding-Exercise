using System;

namespace LegacyApp.Interfaces
{
    public interface IUserValidator
    {
        void ValidateUserInput(string firstName, string surname, string email, DateTime dateOfBirth);
    }
}
