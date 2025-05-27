using System;
using foodCategory;
using Customer.CustomerQueries;
using EntryPoint.Utilities;
using Customer.CustomerEmail;
using MySql.Data.MySqlClient;

namespace Customer
{

    public class CustomerUI
    {
        private static string currentUserEmail = string.Empty;
        public static string GetCurrentUserEmail() => currentUserEmail;
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
                currentUserEmail = email;
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
                    CheckOrderStatus(currentUserEmail);
                    break;
                case 2:
                    ConsoleCenter.WriteCentered("Account Info System");
                    AccountInfo();
                    break;
                case 3:
                    ConsoleCenter.WriteCentered("Exiting Customer System...");
                    Environment.Exit(0);
                    break;
            }
        }

        public enum AssignmentStatus
        {
            Unassign = 0,
            Assigned = 1
        }

        public enum DeliveryStatus
        {
            Preparing = 0,
            Packed = 1,
            OutForDelivery = 2,
            Delivered = 3
        }
        public static void CheckOrderStatus(string email)
        {
            string connectionString = "server=localhost; database=fdom; user=root; password=;";
            string query = @"
                SELECT 
                    o.OrderID,
                    o.FoodID,
                    f.Name AS FoodName,
                    o.Quantity,
                    o.TotalPrice,
                    o.OrderDate,
                    o.AssignmentStatus,
                    o.DeliveryStatus,
                    o.DispatcherID
                FROM customer_order o
                JOIN customer c ON o.CustomerID = c.CustomerID
                JOIN food_menu f ON o.FoodID = f.FoodID
                WHERE c.Email = @Email
                ORDER BY o.OrderDate DESC;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            ConsoleCenter.WriteCentered("No orders found for this email.");
                            return;
                        }

                        while (reader.Read())
                        {
                            AssignmentStatus assignmentStatus = (AssignmentStatus)Convert.ToInt32(reader["AssignmentStatus"]);
                            DeliveryStatus deliveryStatus = (DeliveryStatus)Convert.ToInt32(reader["DeliveryStatus"]);

                            ConsoleCenter.WriteCentered("========== ORDER RECEIPT ==========");
                            Console.WriteLine($"Order ID         : {reader["OrderID"]}");
                            Console.WriteLine($"Food Name        : {reader["FoodName"]}");
                            Console.WriteLine($"Quantity         : {reader["Quantity"]}");
                            Console.WriteLine($"Total Price      : {reader["TotalPrice"]:C}");
                            Console.WriteLine($"Order Date       : {reader["OrderDate"]}");
                            var dispatcherId = reader["DispatcherID"] == DBNull.Value ? "Not yet assigned" : reader["DispatcherID"].ToString();
                            Console.WriteLine($"Dispatcher ID    : {dispatcherId}");
                            Console.WriteLine($"Assignment Status: {assignmentStatus}");
                            Console.WriteLine($"Delivery Status  : {deliveryStatus}");
                            ConsoleCenter.WriteCentered("====================================");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching order status: {ex.Message}");
            }
        }

        private static string FormatOrderReceipt(MySqlDataReader reader)
        {
            var assignmentStatus = (AssignmentStatus)Convert.ToInt32(reader["AssignmentStatus"]);
            var deliveryStatus = (DeliveryStatus)Convert.ToInt32(reader["DeliveryStatus"]);
            var dispatcherId = reader["DispatcherID"] == DBNull.Value ? "Not yet assigned" : reader["DispatcherID"].ToString();

            return
        $@"========== ORDER RECEIPT ==========
        Order ID         : {reader["OrderID"]}
        Food Name        : {reader["FoodName"]}
        Quantity         : {reader["Quantity"]}
        Total Price      : {Convert.ToDecimal(reader["TotalPrice"]):C}
        Order Date       : {reader["OrderDate"]}
        Dispatcher ID    : {dispatcherId}
        Assignment Status: {assignmentStatus}
        Delivery Status  : {deliveryStatus}
        ====================================";
        }

        
        public static void AccountInfo()
        {
            string connectionString = "server=localhost; database=fdom; user=root; password=;";
            string email = currentUserEmail;

            // Query for Customer
            string customerQuery = @"
                SELECT CustomerID, Name, Email, Address, ContactNumber
                FROM customer
                WHERE Email = @Email;";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check Customer
                    using (var customerCmd = new MySqlCommand(customerQuery, connection))
                    {
                        customerCmd.Parameters.AddWithValue("@Email", email);
                        using (var reader = customerCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ConsoleCenter.WriteCentered("====== CUSTOMER INFO ======");
                                Console.WriteLine($"Customer ID   : {reader["CustomerID"]}");
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
    }
}
