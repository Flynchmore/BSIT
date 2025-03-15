using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        while (true)
        {
            Choose.character();
        }
    }
}

class Register
{
    public static List<Register> Users = new List<Register>();

    public string Username { get; private set; }
    public string Password { get; private set; }

    public Register(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public static void accountRegister()
    {
        Console.WriteLine("\n=================== Create a Billing Account ===================");

        Console.Write("Enter your preferred billing name: ");
        string username = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Enter your preferred password: ");
        string password = Console.ReadLine()?.Trim() ?? string.Empty;

        Users.Add(new Register(username, password));

        Console.WriteLine("\nAccount successfully created!");
        Console.Write("Do you like to Login now? Press Y or N: ");
        string proceed = Console.ReadLine()?.Trim() ?? string.Empty;

        if (proceed.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            Login.accountLogin();
        }
        else
        {
            Console.WriteLine("\nReturning to menu...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

class Login
{
    public static void accountLogin()
    {
        Console.WriteLine("\n=================== LOGIN ACCOUNT ===================");

        Console.Write("Enter your username: ");
        string verifyUsername = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Enter your password: ");
        string verifyPassword = Console.ReadLine()?.Trim() ?? string.Empty;

        foreach (var user in Register.Users)
        {
            if (user.Username == verifyUsername && user.Password == verifyPassword)
            {
                Console.WriteLine("\nLogin Successful!");
                Bills.calculation();
                return;
            }
        }

        Console.WriteLine("\nInvalid Username or Password! Please try again.");
        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        Console.Clear();
    }
}

class Bills
{
    public static List<string> billRecords = new List<string>();

    public static void calculation()
    {
        Console.WriteLine("\n================= BILL CONSUMPTION =================");

        Console.Write("Enter your Rate per kW/h: ");
        double kwtt = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter your Consumption of the Previous Month: ");
        double prev = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter your Consumption of the Current Month: ");
        double curr = Convert.ToDouble(Console.ReadLine());

        double bill = curr * kwtt;
        double diff = Math.Abs(bill - prev);

        string record = $"User Bill: Rate per kwtt: {kwtt}, Previous Month Bill: {prev}, Current Month Bill: {curr}, Total Bill for this Month: \u20B1{bill:F2}, Difference of Previous and Current Month Bill: \u20B1{diff:F2}";
        billRecords.Add(record);

        Console.WriteLine($"\nYour Total Consumption Bill for this Month is: \u20B1{bill:F2}");
        Console.WriteLine($"The difference between your current and previous month bill is: \u20B1{diff:F2}");
    }
}

class Choose
{
    public static void character()
    {
        Console.WriteLine("================== Billing System Menu ==================");
        Console.WriteLine("1. Register a Billing Account");
        Console.WriteLine("2. Login to your Billing Account");
        Console.WriteLine("3. View All Bill Records");
        Console.WriteLine("4. Exit the Program");
        Console.Write("Choose a number to proceed: ");

        string number = Console.ReadLine() ?? string.Empty;

        if (number == "1")
        {
            Console.Clear();
            Register.accountRegister();
        }
        else if (number == "2")
        {
            Console.Clear();
            Login.accountLogin();
        }
        else if (number == "3")
        {
            Console.Clear();
            foreach (var record in Bills.billRecords)
            {
                Console.WriteLine(record);
            }
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            Console.Clear();
        }
        else if (number == "4")
        {
            Console.WriteLine("Thank you for using the program!");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Oops! Invalid Choice. Try again.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}