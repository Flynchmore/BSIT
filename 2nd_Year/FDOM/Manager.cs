using System;
using Manager.ManagerQueries;
using EntryPoint.Utilities;
using Manager.ManagerEmail;
using MySql.Data.MySqlClient;
using Dispatcher.DispatcherEmail;
using Customer;

namespace Manager {

public class ManagerUI
{
    private static string currentUserEmail = string.Empty;

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
            currentUserEmail = email;
            Managerfunction();
        }
        else
        {
            ConsoleCenter.WriteCentered("Invalid email or password. Please try again.");
        }
    }
    public static void Managerfunction()
    {
        string[] options = { "Restaurant Database Menu", "Assign Delivery Personnel", "Account Info", "Exit" };
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
               AccountInfo();
                break;
            case 3:
                Environment.Exit(0);
                break;
        }
    }

    private static void ManageRestaurantMenu()
    {
        string[] options = { "Add Menu Item", "Update Menu Item", "Delete Menu Item", "Exit"};

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
                Environment.Exit(0);
                break;
        }
    }

    private static void AddMenuItem()
    {
        ConsoleCenter.WriteCentered("\nAdd Menu Item");
        string name = GetHelper.GetCenteredInput("Enter Item Name: ");
        decimal price = decimal.Parse(GetHelper.GetCenteredInput("Enter Item Price: "));
        int category = int.Parse(GetHelper.GetCenteredInput("Enter Item Category Number (0.Breakfast, 1.Lunch, 2.Dinner, 3.Dessert): "));
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
        int category = int.Parse(GetHelper.GetCenteredInput("Enter new Item Category (0.Breakfast, 1.Lunch, 2.Dinner, 3.Dessert): "));
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

   // Example for Manager.cs or wherever your manager logic resides
    public static void AssignDeliveryPersonnel()
    {
        string connectionString = "server=localhost; database=fdom; user=root; password=;";

        // 1. Show unassigned orders
        string unassignedOrdersQuery = @"
            SELECT o.OrderID, c.Name AS CustomerName, f.Name AS FoodName, o.Quantity, o.OrderDate
            FROM customer_order o
            JOIN customer c ON o.CustomerID = c.CustomerID
            JOIN food_menu f ON o.FoodID = f.FoodID
            WHERE o.DispatcherID IS NULL OR o.AssignmentStatus = 0;";

        // 2. Show available dispatchers
        string dispatcherQuery = "SELECT DispatcherID, Name FROM dispatcher;";

        try
        {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Show unassigned orders
                    ConsoleCenter.WriteCentered("Unassigned Customer Orders:");
                    using (var cmd = new MySqlCommand(unassignedOrdersQuery, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"OrderID: {reader["OrderID"]}, Customer: {reader["CustomerName"]}, Food: {reader["FoodName"]}, Qty: {reader["Quantity"]}, Date: {reader["OrderDate"]}");
                        }
                    }

                    // Show dispatchers
                    ConsoleCenter.WriteCentered("Available Dispatchers:");
                    using (var cmd = new MySqlCommand(dispatcherQuery, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"DispatcherID: {reader["DispatcherID"]}, Name: {reader["Name"]}");
                        }
                    }

                    // Get input
                    ConsoleCenter.WriteCentered("Enter OrderID to assign:");
                    int orderId = int.Parse(Console.ReadLine() ?? "0");
                    ConsoleCenter.WriteCentered("Enter DispatcherID to assign:");
                    int dispatcherId = int.Parse(Console.ReadLine() ?? "0");

                    // Assign dispatcher
                    string assignQuery = @"
                    UPDATE customer_order
                    SET DispatcherID = @DispatcherID, AssignmentStatus = 1
                    WHERE OrderID = @OrderID;";
                    using (var assignCmd = new MySqlCommand(assignQuery, connection))
                    {
                        assignCmd.Parameters.AddWithValue("@DispatcherID", dispatcherId);
                        assignCmd.Parameters.AddWithValue("@OrderID", orderId);
                        int rows = assignCmd.ExecuteNonQuery();
                        if (rows > 0)
                            ConsoleCenter.WriteCentered("Dispatcher assigned successfully!");
                        else
                            ConsoleCenter.WriteCentered("Assignment failed. Check IDs.");
                    }
                
                    // After successful assignment (rows > 0)
                    string dispatcherEmail = "";
                    using (var dispatcherEmailCmd = new MySqlCommand("SELECT Email FROM dispatcher WHERE DispatcherID = @DispatcherID", connection))
                    {
                        dispatcherEmailCmd.Parameters.AddWithValue("@DispatcherID", dispatcherId);
                        using (var reader = dispatcherEmailCmd.ExecuteReader())
                        {
                            if (reader.Read())
                                dispatcherEmail = reader["Email"].ToString() ?? string.Empty;
                        }
                    }

                    string orderDetailsQuery = @"
                        SELECT o.OrderID, c.Name AS CustomerName, f.Name AS FoodName, o.Quantity, o.OrderDate
                        FROM customer_order o
                        JOIN customer c ON o.CustomerID = c.CustomerID
                        JOIN food_menu f ON o.FoodID = f.FoodID
                        WHERE o.OrderID = @OrderID;";
                    using (var orderDetailsCmd = new MySqlCommand(orderDetailsQuery, connection))
                    {
                        orderDetailsCmd.Parameters.AddWithValue("@OrderID", orderId);
                        using (var reader = orderDetailsCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string orderInfo = $@"
                                                ========== NEW DELIVERY ASSIGNMENT ==========
                                                OrderID   : {reader["OrderID"]}
                                                Customer  : {reader["CustomerName"]}
                                                Food      : {reader["FoodName"]}
                                                Quantity  : {reader["Quantity"]}
                                                OrderDate : {reader["OrderDate"]}
                                                ============================================";
                                DispatcherEmailOperations.SendOrderAssignmentEmail(dispatcherEmail, "New Delivery Assignment", orderInfo);
                            }
                        }
                    }

                }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    
    public static void UpdateOrderDeliveryStatus()
    {
        string connectionString = "server=localhost; database=fdom; user=root; password=;";

        ConsoleCenter.WriteCentered("Enter OrderID to update:");
        int orderId = int.Parse(Console.ReadLine() ?? "0");

        ConsoleCenter.WriteCentered("Choose new Delivery Status:");
        Console.WriteLine("0 - Preparing\n1 - Packed\n2 - OutForDelivery\n3 - Delivered");
        int newStatus = int.Parse(Console.ReadLine() ?? "0");

        string updateQuery = @"
            UPDATE customer_order
            SET DeliveryStatus = @DeliveryStatus
            WHERE OrderID = @OrderID;";

        string customerEmail = "";
        string orderDetails = "";

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Update delivery status
                using (var updateCmd = new MySqlCommand(updateQuery, connection))
                {
                    updateCmd.Parameters.AddWithValue("@DeliveryStatus", newStatus);
                    updateCmd.Parameters.AddWithValue("@OrderID", orderId);
                    int rows = updateCmd.ExecuteNonQuery();
                    if (rows > 0)
                        ConsoleCenter.WriteCentered("Delivery status updated successfully!");
                    else
                    {
                        ConsoleCenter.WriteCentered("Update failed. Check OrderID.");
                        return;
                    }
                }

                // Fetch customer email and order info
                string fetchQuery = @"
                    SELECT c.Email, o.OrderID, f.Name AS FoodName, o.Quantity, o.TotalPrice, o.OrderDate, o.DeliveryStatus
                    FROM customer_order o
                    JOIN customer c ON o.CustomerID = c.CustomerID
                    JOIN food_menu f ON o.FoodID = f.FoodID
                    WHERE o.OrderID = @OrderID;";
                using (var fetchCmd = new MySqlCommand(fetchQuery, connection))
                {
                    fetchCmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (var reader = fetchCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customerEmail = reader["Email"].ToString() ?? "";
                            var deliveryStatus = (CustomerUI.DeliveryStatus)Convert.ToInt32(reader["DeliveryStatus"]);
                            orderDetails = $@"
                                        ========== ORDER STATUS UPDATE ==========
                                        Order ID        : {reader["OrderID"]}
                                        Food Name       : {reader["FoodName"]}
                                        Quantity        : {reader["Quantity"]}
                                        Total Price     : {reader["TotalPrice"]:C}
                                        Order Date      : {reader["OrderDate"]}
                                        New Status      : {deliveryStatus}
                                        =========================================";
                        }
                    }
                }
            }

            // Send email notification to customer
            if (!string.IsNullOrEmpty(customerEmail))
            {
                Customer.CustomerEmail.CustomerEmailOperations.SendOrderReceiptEmail(
                    customerEmail,
                    "Order Delivery Status Updated",
                    orderDetails
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    public static void AccountInfo()
        {
            string connectionString = "server=localhost; database=fdom; user=root; password=;";
            string email = currentUserEmail;

            // Query for Manager
            string managerQuery = @"
            SELECT ManagerID, Name, Email, Address, ContactNumber
            FROM manager
            WHERE Email = @Email;";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check Manager
                    using (var managerCmd = new MySqlCommand(managerQuery, connection))
                    {
                        managerCmd.Parameters.AddWithValue("@Email", email);
                        using (var reader = managerCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ConsoleCenter.WriteCentered("====== MANAGER INFO ======");
                                Console.WriteLine($"Manager ID   : {reader["ManagerID"]}");
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
