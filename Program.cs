using System;
using Choice;

class Program
{
    static void Main()
    {
        bool exit = false;
        do
        {
            RoleChoice.User();
            Console.Write("Do you want to exit? (Y/N): ");
            string choice = Console.ReadLine() ?? string.Empty;
            exit = choice.Equals("Y", StringComparison.CurrentCultureIgnoreCase);
        } while (!exit);

        Console.WriteLine("Thank you for using the system. Goodbye!");
    }
}