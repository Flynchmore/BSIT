using System;
using UserOne;
using UserTwo;
using UserThree;

namespace Choice{
public static class RoleChoice
{
    public static void User()
    {
        bool exitRole = false;
        do
        {
            Console.WriteLine("=============================================== \nChoose your role ===============================================");
            Console.WriteLine("[1]. Customer");
            Console.WriteLine("[2]. Dispatcher");
            Console.WriteLine("[3]. Manager");
            Console.WriteLine("[4]. Exit");
            Console.Write("\nAnswer: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    CustomerUser.Customer();
                    break;
                case "2":
                    DispatcherUser.Dispatcher();
                    break;
                case "3":
                    ManagerUser.Manager();
                    break;
                case "4":
                    exitRole = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        } while (!exitRole);
    }
}
}