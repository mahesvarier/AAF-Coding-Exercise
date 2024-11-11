using LegacyApp.Interfaces;
using LegacyApp.Models;

public static class UserCreditManager
{
    private static readonly string VeryImportantClient = "VeryImportantClient";
    private static readonly string ImportantClient = "ImportantClient";

    public static void SetUserCreditLimit(User user, Client client, IUserCreditService userCreditService)
    {
        if (client.Name == VeryImportantClient)
        {
            user.HasCreditLimit = false;
        }
        else
        {
            user.HasCreditLimit = true;
            int creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);

            if (client.Name == ImportantClient)
            {
                creditLimit *= 2;
            }

            user.CreditLimit = creditLimit;
        }
    }
}
