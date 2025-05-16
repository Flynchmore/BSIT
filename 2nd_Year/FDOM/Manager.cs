using System;
using Manager.ManagerQueries;
using EntryPoint.Utilities;
using Manager.ManagerEmail;

namespace Manager {

public class ManagerUI
{
    public static void ManagerRegister()
    {
        ConsoleCenter.WriteCentered("\nManager Registration");
        string name = GetHelper.GetCenteredInput("Name: ");
        string email = GetHelper.GetCenteredInput("Email: ");
        string password = GetHelper.GetCenteredInput("Password: ");
        string address = GetHelper.GetCenteredInput("Address: ");
        string contactNumber = GetHelper.GetCenteredInput("Contact Number: ");

        // Insert the manager into the database
        ManagerDB.managerRegister(name, email, password, address, contactNumber);

        // Send the email notification for the manager
        ManagerEmailOperations.ManagerAccountRegistrationEmail();
    }

    public static void ManagerLogin()
    {
        ConsoleCenter.WriteCentered("\nManager Login");
        string email = GetHelper.GetCenteredInput("Email: ");
        string password = GetHelper.GetCenteredInput("Password: ");

        // Check if the manager exists in the database
        if (ManagerDB.ValidateManagerCredentials(email, password))
        {
            ConsoleCenter.WriteCentered("Login successful!");
            Managerfunction();
        }
        else
        {
            ConsoleCenter.WriteCentered("Invalid email or password. Please try again.");
        }
    }
    public static void Managerfunction()
    {
        string[] options = { "Restaurant Database Menu", "Assign Dispatcher Personnel", "Account Info", "Exit" };
        userChoice.DisplayMenu("Welcome to the Manager System!", options, HandleSelection);
    }

    private static void HandleSelection(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                ManageRestaurantMenu();
                break;
            case 1:
               AssignDeliveryPersonnel();
                break;
            case 2:
               AccountInfoSystem();
                break;
            case 3:
                ConsoleCenter.WriteCentered("Exiting Manager System...");
                Environment.Exit(0);
                break;
        }
    }

    private static void ManageRestaurantMenu()
    {
        string[] options = { "Add Menu Item", "Update Menu Item", "Delete Menu Item", "Assign Delivery Personnel", "Account Info", "Exit"};

        userChoice.DisplayMenu("Managing Restaurant Menu", options, HandleRestaurantMenuSelection);
    }

    private static void HandleRestaurantMenuSelection(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                AddMenuItem();
                break;
            case 1:
                UpdateMenuItem();
                break;
            case 2:
                DeleteMenuItem();
                break;
            case 3:
                AssignDeliveryPersonnel();
                break;
            case 4:
                AccountInfoSystem();
                break;
            case 5:
                ConsoleCenter.WriteCentered("Exiting Manager System...");
                Environment.Exit(0);
                break;
        }
    }

    private static void AddMenuItem()
    {
        ConsoleCenter.WriteCentered("\nAdd Menu Item");
        string name = GetHelper.GetCenteredInput("Enter Item Name: ");
        decimal price = decimal.Parse(GetHelper.GetCenteredInput("Enter Item Price: "));
        int category = int.Parse(GetHelper.GetCenteredInput("Enter Item Category Number (1.Breakfast, 2.Lunch, 3.Dinner, 4.Dessert): "));
        int stacks = int.Parse(GetHelper.GetCenteredInput("Enter Item Stacks: "));

        ManagerDB.AddMenuItem(name, price, category, stacks);
    }

    private static void UpdateMenuItem()
    {
        ManagerDB.ReadMenuItems(); 
        ConsoleCenter.WriteCentered("\n==========================================================");
        ConsoleCenter.WriteCentered("\nUpdate Menu Item");
        int id = int.Parse(GetHelper.GetCenteredInput("Enter Item ID to update: "));
        string name = GetHelper.GetCenteredInput("Enter new Item Name: ");
        decimal price = decimal.Parse(GetHelper.GetCenteredInput("Enter new Item Price: "));
        int category = int.Parse(GetHelper.GetCenteredInput("Enter new Item Category (1.Breakfast, 2.Lunch, 3.Dinner, 4.Dessert): "));
        int stacks = int.Parse(GetHelper.GetCenteredInput("Enter new Item Stacks: "));

        ManagerDB.UpdateMenuItem(id, name, price, category, stacks);
    }

    private static void DeleteMenuItem()
    {
        ManagerDB.ReadMenuItems(); 
        ConsoleCenter.WriteCentered("\n==========================================================");
        ConsoleCenter.WriteCentered("\nDelete Menu Item");
        int id = int.Parse(GetHelper.GetCenteredInput("Enter Item ID to Delete: "));

        ManagerDB.DeleteMenuItem(id);
    }

    private static void AssignDeliveryPersonnel()
    {

    }
    private static void AccountInfoSystem()
    {
        
    }
}
}
