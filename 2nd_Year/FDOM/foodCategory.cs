using System;
using EntryPoint.Utilities;
using MySql.Data.MySqlClient;
using Customer.CustomerEmail;
using Customer;

namespace foodCategory;

public class FoodUI
{
    public static void Foodfunction()
    {
        string[] options = { "Breakfast", "Lunch", "Dinner", "Dessert", "Exit" };
        userChoice.DisplayMenu("Welcome to the Food Category System!", options, HandleSelection);
    }

    private static void HandleSelection(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                ConsoleCenter.WriteCentered("Breakfast System");
                Breakfast();
                break;
            case 1:
                ConsoleCenter.WriteCentered("Lunch System");
                Lunch();
                break;
            case 2:
                ConsoleCenter.WriteCentered("Dinner System");
                Dinner();
                break;
            case 3:
                ConsoleCenter.WriteCentered("Dessert System");
                Dessert();
                break;
            case 4:
                ConsoleCenter.WriteCentered("Exiting Food Category System...");
                Environment.Exit(0);
                break;
        }
    }

    public enum FoodCategory
    {
        Breakfast = 0,
        Lunch = 1,
        Dinner = 2,
        Dessert = 3
    }

    public static void ReadMenuItemsByCategory(int category)
    {
        string connectionString = "server=localhost; database=fdom; user=root; password=;"; //Connect to the phpMyAdmin app
        string query = "SELECT * FROM food_menu WHERE Category = @Category;";
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Category", category);
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nMenu Items:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"FoodID: {reader["FoodID"]}, Name: {reader["Name"]}, Price: {reader["Price"]}, Stacks: {reader["Stacks"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading menu items: {ex.Message}");
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
    public static void OrderFood(int foodId, int quantity)
    {
        string connectionString = "server=localhost; database=fdom; user=root; password=;";
        string selectQuery = "SELECT Name, Price, Stacks FROM food_menu WHERE FoodID = @FoodID;";
        string updateQuery = "UPDATE food_menu SET Stacks = Stacks - @Quantity WHERE FoodID = @FoodID AND Stacks >= @Quantity;";
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string foodName = string.Empty;
                decimal price = 0;
                int currentStacks = 0;

                // Get food details and check stock
                MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@FoodID", foodId);
                MySqlDataReader reader = selectCommand.ExecuteReader();
                if (!reader.Read())
                {
                    ConsoleCenter.WriteCentered("Invalid FoodID selected.");
                    reader.Close();
                    return;
                }
                foodName = reader["Name"].ToString() ?? string.Empty;
                price = Convert.ToDecimal(reader["Price"]);
                currentStacks = Convert.ToInt32(reader["Stacks"]);
                reader.Close();

                if (currentStacks < quantity)
                {
                    ConsoleCenter.WriteCentered($"Not enough stock. Only {currentStacks} available.");
                    return;
                }

                // Update stock
                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@FoodID", foodId);
                updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                int rowsAffected = updateCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Dynamically get current user's email from session or context
                    string currentUserEmail = Customer.CustomerUI.GetCurrentUserEmail();
                    if (string.IsNullOrEmpty(currentUserEmail))
                    {
                        ConsoleCenter.WriteCentered("User session not found. Please log in again.");
                        return;
                    }

                    // Get CustomerID by email
                    int customerId = -1;
                    MySqlCommand getCustomerCmd = new MySqlCommand("SELECT CustomerID FROM customer WHERE Email = @Email", connection);
                    getCustomerCmd.Parameters.AddWithValue("@Email", currentUserEmail);
                    MySqlDataReader custReader = getCustomerCmd.ExecuteReader();
                    if (custReader.Read())
                        customerId = Convert.ToInt32(custReader["CustomerID"]);
                    custReader.Close();

                    if (customerId == -1)
                    {
                        ConsoleCenter.WriteCentered("Customer not found.");
                        return;
                    }

                    // Insert order into customer_order
                    MySqlCommand insertOrderCmd = new MySqlCommand(@"
                        INSERT INTO customer_order 
                        (CustomerID, FoodID, Quantity, TotalPrice, OrderDate, AssignmentStatus, DeliveryStatus)
                        VALUES (@CustomerID, @FoodID, @Quantity, @TotalPrice, NOW(), 0, 0)", connection);
                    insertOrderCmd.Parameters.AddWithValue("@CustomerID", customerId);
                    insertOrderCmd.Parameters.AddWithValue("@FoodID", foodId);
                    insertOrderCmd.Parameters.AddWithValue("@Quantity", quantity);
                    insertOrderCmd.Parameters.AddWithValue("@TotalPrice", price * quantity);
                    insertOrderCmd.ExecuteNonQuery();

                    // After inserting the order into customer_order

                    string latestOrderQuery = @"
                        SELECT o.OrderID, o.FoodID, f.Name AS FoodName, o.Quantity, o.TotalPrice, o.OrderDate,
                            o.AssignmentStatus, o.DeliveryStatus, o.DispatcherID
                        FROM customer_order o
                        JOIN food_menu f ON o.FoodID = f.FoodID
                        WHERE o.CustomerID = @CustomerID
                        ORDER BY o.OrderDate DESC
                        LIMIT 1;";

                    using (var latestOrderCmd = new MySqlCommand(latestOrderQuery, connection))
                    {
                        latestOrderCmd.Parameters.AddWithValue("@CustomerID", customerId);
                        using (var latestOrderReader = latestOrderCmd.ExecuteReader())
                        {
                            if (latestOrderReader.Read())
                            {
                                string receipt = CustomerUI.FormatOrderReceipt(latestOrderReader);
                                CustomerEmailOperations.SendOrderReceiptEmail(currentUserEmail, "Your Order Receipt", receipt);
                            }
                        }
                    }

                    decimal total = price * quantity;
                    ConsoleCenter.WriteCentered("Order placed successfully!");
                    ConsoleCenter.WriteCentered("========== RECEIPT ==========");
                    ConsoleCenter.WriteCentered($"FoodID   : {foodId}");
                    ConsoleCenter.WriteCentered($"Name     : {foodName}");
                    ConsoleCenter.WriteCentered($"Quantity : {quantity}");
                    ConsoleCenter.WriteCentered($"Price    : {price:C}");
                    ConsoleCenter.WriteCentered($"Total    : {total:C}");
                    ConsoleCenter.WriteCentered("=============================");
                }
                else
                {
                    ConsoleCenter.WriteCentered("Failed to place order. Please try again.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while processing the order: {ex.Message}");
        }
    }
    public static void Breakfast()
    {
        ReadMenuItemsByCategory((int)FoodCategory.Breakfast);
        ConsoleCenter.WriteCentered("==========================================================");
        ConsoleCenter.WriteCentered("Enter the FoodID of the item you want to order: ");
        if (!int.TryParse(Console.ReadLine(), out int foodId))
        {
            ConsoleCenter.WriteCentered("Invalid FoodID input.");
            return;
        }
        ConsoleCenter.WriteCentered("Enter the Quantity of the food item: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            ConsoleCenter.WriteCentered("Invalid quantity input.");
            return;
        }
        OrderFood(foodId, quantity);
    }

    public static void Lunch()
    {
        ReadMenuItemsByCategory((int)FoodCategory.Lunch);
        ConsoleCenter.WriteCentered("==========================================================");
        ConsoleCenter.WriteCentered("Enter the FoodID of the item you want to order: ");
        if (!int.TryParse(Console.ReadLine(), out int foodId))
        {
            ConsoleCenter.WriteCentered("Invalid FoodID input.");
            return;
        }
        ConsoleCenter.WriteCentered("Enter the Quantity of the food item: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            ConsoleCenter.WriteCentered("Invalid quantity input.");
            return;
        }
        OrderFood(foodId, quantity);
    }

    public static void Dinner()
    {
        ReadMenuItemsByCategory((int)FoodCategory.Dinner);
        ConsoleCenter.WriteCentered("==========================================================");
        ConsoleCenter.WriteCentered("Enter the FoodID of the item you want to order: ");
        if (!int.TryParse(Console.ReadLine(), out int foodId))
        {
            ConsoleCenter.WriteCentered("Invalid FoodID input.");
            return;
        }
        ConsoleCenter.WriteCentered("Enter the Quantity of the food item: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            ConsoleCenter.WriteCentered("Invalid quantity input.");
            return;
        }
        OrderFood(foodId, quantity);
    }

    public static void Dessert()
    {
        ReadMenuItemsByCategory((int)FoodCategory.Dessert);
        ConsoleCenter.WriteCentered("==========================================================");
        ConsoleCenter.WriteCentered("Enter the FoodID of the item you want to order: ");
        if (!int.TryParse(Console.ReadLine(), out int foodId))
        {
            ConsoleCenter.WriteCentered("Invalid FoodID input.");
            return;
        }
        ConsoleCenter.WriteCentered("Enter the Quantity of the food item: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            ConsoleCenter.WriteCentered("Invalid quantity input.");
            return;
        }
        OrderFood(foodId, quantity);
    }

}
