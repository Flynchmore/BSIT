using System;
using foodCategory;
using Customer.CustomerQueries;
using EntryPoint.Utilities;
using Customer.CustomerEmail;

namespace Customer {

public class CustomerUI
{
   public static void CustomerRegister()
    {
        ConsoleCenter.WriteCentered("\nCustomer Registration");
        string name = GetHelper.GetCenteredInput("Name: ");
        string email = GetHelper.GetCenteredInput("Email: ");
        string password = GetHelper.GetCenteredInput("Password: ");
        string address = GetHelper.GetCenteredInput("Address: ");
        string contactNumber = GetHelper.GetCenteredInput("Contact Number: ");

        // Insert the customer into the database
        CustomerDB.customerRegister(name, email, password, address, contactNumber);

        // Send the email notification for the customer
        CustomerEmailOperations.CustomerAccountRegistrationEmail();
    }

    public static void CustomerLogin()
    {
        ConsoleCenter.WriteCentered("\nCustomer Login");
        string email = GetHelper.GetCenteredInput("Email: ");
        string password = GetHelper.GetCenteredInput("Password: ");

        // Check if the customer exists in the database
        if (CustomerDB.ValidateCustomerCredentials(email, password))
        {
            ConsoleCenter.WriteCentered("Login successful!");
            Customerfunction();
        }
        else
        {
            ConsoleCenter.WriteCentered("Invalid email or password. Please try again.");
        }
    }
    public static void Customerfunction()
    {
        string[] options = { "Order Food", "Check Order Status", "Account Info", "Exit" };
        userChoice.DisplayMenu("Welcome to the Customer System!", options, HandleSelection);
    }

    private static void HandleSelection(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                ConsoleCenter.WriteCentered("Order Food System");
                FoodUI.Foodfunction();
                break;
            case 1:
                ConsoleCenter.WriteCentered("Check Order Status System");
                break;
            case 2:
                ConsoleCenter.WriteCentered("Account Info System");
                break;
            case 3:
                ConsoleCenter.WriteCentered("Exiting Customer System...");
                Environment.Exit(0);
                break;
        }
    }
}
}