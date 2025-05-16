using System;
using Manager;
using Dispatcher.DispatcherQueries;
using EntryPoint.Utilities;
using Dispatcher.DispatcherEmail;

namespace Dispatcher {

public class DispatcherUI
{
    public static void DispatcherRegister()
    {
        ConsoleCenter.WriteCentered("\nDispatcher Registration");
        string name = GetHelper.GetCenteredInput("Name: ");
        string email = GetHelper.GetCenteredInput("Email: ");
        string password = GetHelper.GetCenteredInput("Password: ");
        string address = GetHelper.GetCenteredInput("Address: ");
        string contactNumber = GetHelper.GetCenteredInput("Contact Number: ");

        // Insert the dispatcher into the database
        DispatcherDB.dispatcherRegister(name, email, password, address, contactNumber);

        // Send the email notification for the dispatcher
        DispatcherEmailOperations.DispatcherAccountRegistrationEmail();
    }

    public static void DispatcherLogin()
    {
        ConsoleCenter.WriteCentered("\nDispatcher Login");
        string email = GetHelper.GetCenteredInput("Email: ");
        string password = GetHelper.GetCenteredInput("Password: ");

        // Check if the dispatcher exists in the database
        if (DispatcherDB.ValidateDispatcherCredentials(email, password))
        {
            ConsoleCenter.WriteCentered("Login successful!");
            Dispatcherfunction();
        }
        else
        {
            ConsoleCenter.WriteCentered("Invalid email or password. Please try again.");
        }
    }
    public static void Dispatcherfunction()
    {
        string[] options = { "Food Delivery Status", "Account Info", "Exit" };
        userChoice.DisplayMenu("Welcome to the Dispatcher System!", options, HandleSelection);
    }

    private static void HandleSelection(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                ConsoleCenter.WriteCentered("Food Delivery Status System");
                break;
            case 1:
                ConsoleCenter.WriteCentered("Account Info System");
                break;
            case 2:
                ConsoleCenter.WriteCentered("Exiting Dispatcher System...");
                Environment.Exit(0);
                break;
        }
    }
}
}