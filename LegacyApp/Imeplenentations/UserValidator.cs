using System;

namespace LegacyApp.Imeplenentations
{
    public static class UserValidator
    {
        public static void ValidateUserInput(string firstName, string surname, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname))
            {
                throw new InvalidOperationException("User firstname and surname are required.");
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new InvalidOperationException("User email is invalid.");
            }

            int age = CalculateAge(dateOfBirth);
            if (age < 21)
            {
                throw new InvalidOperationException("User should be older than 21 years.");
            }
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }
            return age;
        }
    }
}
