using System;
using User; // Namespace from dbInsertion.cs

class Program
{
    static void Main()
    {
        Console.WriteLine("Registration");

        // Collect user input
        Console.Write("Whole Name: ");
        string wholename = Console.ReadLine() ?? string.Empty;

        Console.Write("Student Number: ");
        string studentnum = Console.ReadLine() ?? string.Empty;

        Console.Write("Age: ");
        string ageInput = Console.ReadLine() ?? string.Empty;
        if (!int.TryParse(ageInput, out int age))
        {
            Console.WriteLine("Invalid age. Please enter a valid number.");
            return;
        }

        Console.Write("Cellphone Number: ");
        string cellphonenumInput = Console.ReadLine() ?? string.Empty;
        if (!long.TryParse(cellphonenumInput, out long cellphonenum))
        {
            Console.WriteLine("Invalid cellphone number. Please enter a valid number.");
            return;
        }

        // Use the InsertNewData class from dbInsertion.cs
        Register.InsertNewData register = new Register.InsertNewData();
        register.InsertData(wholename, studentnum, age, cellphonenum.ToString());
    }
}