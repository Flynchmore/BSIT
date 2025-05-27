using System;
using Manager;
using Dispatcher.DispatcherQueries;
using EntryPoint.Utilities;
using Dispatcher.DispatcherEmail;
using MySql.Data.MySqlClient;

namespace Dispatcher {

    public class DispatcherUI
    {
        private static string currentUserEmail = string.Empty;

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
                currentUserEmail = email;
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
                    FoodDeliveryStatus();
                    break;
                case 1:
                    ConsoleCenter.WriteCentered("Account Info System");
                    AccountInfo();
                    break;
                case 2:
                    ConsoleCenter.WriteCentered("Exiting Dispatcher System...");
                    Environment.Exit(0);
                    break;
            }
        }

        public static void AccountInfo()
        {
            string connectionString = "server=localhost; database=fdom; user=root; password=;";
            string email = currentUserEmail;

            // Query for Customer
            string dispatcherQuery = @"
                SELECT DispatcherID, Name, Email, Address, ContactNumber
                FROM dispatcher
                WHERE Email = @Email;";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check Dispatcher
                    using (var dispatcherCmd = new MySqlCommand(dispatcherQuery, connection))
                    {
                        dispatcherCmd.Parameters.AddWithValue("@Email", email);
                        using (var reader = dispatcherCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ConsoleCenter.WriteCentered("====== DISPATCHER INFO ======");
                                Console.WriteLine($"Dispatcher ID   : {reader["DispatcherID"]}");
                                Console.WriteLine($"Name          : {reader["Name"]}");
                                Console.WriteLine($"Email         : {reader["Email"]}");
                                Console.WriteLine($"Address       : {reader["Address"]}");
                                Console.WriteLine($"ContactNumber : {reader["ContactNumber"]}");
                                ConsoleCenter.WriteCentered("==========================");
                                return;
                            }
                        }
                    }

                    ConsoleCenter.WriteCentered("No account information found for this email.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching account info: {ex.Message}");
            }
        }

        public static void FoodDeliveryStatus()
        {
            string connectionString = "server=localhost; database=fdom; user=root; password=;";
            string email = currentUserEmail;

            // Get DispatcherID for current user
            int dispatcherId = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT DispatcherID FROM dispatcher WHERE Email = @Email;", connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            dispatcherId = Convert.ToInt32(reader["DispatcherID"]);
                    }
                }

                if (dispatcherId == -1)
                {
                    ConsoleCenter.WriteCentered("Dispatcher not found.");
                    return;
                }

                // Fetch assigned orders
                string assignedOrdersQuery = @"
                    SELECT o.OrderID, c.Name AS CustomerName, f.Name AS FoodName, o.Quantity, o.OrderDate, o.DeliveryStatus
                    FROM customer_order o
                    JOIN customer c ON o.CustomerID = c.CustomerID
                    JOIN food_menu f ON o.FoodID = f.FoodID
                    WHERE o.DispatcherID = @DispatcherID AND o.AssignmentStatus = 1;";

                using (var cmd = new MySqlCommand(assignedOrdersQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@DispatcherID", dispatcherId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        ConsoleCenter.WriteCentered("Assigned Orders:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"OrderID: {reader["OrderID"]}, Customer: {reader["CustomerName"]}, Food: {reader["FoodName"]}, Qty: {reader["Quantity"]}, Date: {reader["OrderDate"]}, Status: {reader["DeliveryStatus"]}");
                        }
                    }
                }
            }
        }
    }
}
