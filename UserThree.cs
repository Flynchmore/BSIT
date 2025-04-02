using System;
using System.Security.Cryptography; // For hashing
using System.Text; 
using MySql.Data.MySqlClient; // Import MySQL library

namespace UserThree
{
    public static class ManagerUser
    {
        private static string connString = "Server=localhost;Database=midterm;User ID=root;Password=;";

        public static void Manager()
        {
            Console.WriteLine("=============================================== \nWelcome to the Manager System! ===============================================");
            Console.Write("Do you already have an account? (Y/N): ");
            string hasAccount = Console.ReadLine() ?? string.Empty;

            if (hasAccount.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                LoginManager();
            }
            else
            {
                RegisterManager();
            }
        }

        private static void RegisterManager()
        {
            Console.WriteLine("=============================================== \nRegister a New Manager Account ===============================================");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            if (CheckIfManagerExists(email))
            {
                Console.WriteLine("\nAn account with this email already exists. Please log in instead.");
                return;
            }

            // Hash the password before storing it
            string hashedPassword = HashPassword(password);

            // Insert manager data into the database
            InsertManager(name, hashedPassword, email);

            Console.Clear();
            Console.WriteLine("Account created successfully!\n Welcome, " + name + "!");
            ShowManagerMenu();
        }

        private static void LoginManager()
        {
            Console.WriteLine("=============================================== \nLog In to Your Manager Account ===============================================");
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;

             // Hash the entered password before checking it
            string hashedPassword = HashPassword(password);

            if (AuthenticateManager(email, hashedPassword))
            {
                Console.Clear();
                Console.WriteLine("\nLogin successful! Welcome back!");
                ShowManagerMenu();
            }
            else
            {
                Console.WriteLine("\nInvalid email or password. Please try again.");
            }
        }

        private static bool CheckIfManagerExists(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM manager WHERE Email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error checking manager existence: " + ex.Message);
                    return false;
                }
            }
        }

       private static bool AuthenticateManager(string email, string hashedPassword)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM manager WHERE Email = @Email AND Password = @Password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error authenticating manager: " + ex.Message);
                    return false;
                }
            }
        }

       private static void InsertManager(string name, string hashedPassword, string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connected to the database!");

                    string query = "INSERT INTO manager (Name, Password, Email) VALUES (@name, @password, @Email)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Manager data inserted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to insert manager data.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        
        private static void ManageRestaurantMenu()
        {
            // Existing functionality for managing the restaurant menu
            Console.WriteLine("=============================================== \nRestaurant Menu Management ===============================================");
            Console.WriteLine("[1]. Add Menu Item");
            Console.WriteLine("[2]. Update Menu Item");
            Console.WriteLine("[3]. Delete Menu Item");
            Console.WriteLine("[4]. Exit");
            Console.Write("\nChoose an option: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    AddMenuItem();
                    break;
                case "2":
                    UpdateMenuItem();
                    break;
                case "3":
                    DeleteMenuItem();
                    break;
                case "4":
                    Console.WriteLine("Exiting Restaurant Menu Management...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

      private static void AddMenuItem()
        {
            Console.WriteLine("=============================================== \nAdd New Menu Item ===============================================");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Category: ");
            string category = Console.ReadLine() ?? string.Empty;
            Console.Write("Price: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.Write("\nQuantity: ");
                if (int.TryParse(Console.ReadLine(), out int quantity))
                {
                    Console.Write("Status (Available/Out of Stock): ");
                    string status = Console.ReadLine() ?? string.Empty;

                    Console.WriteLine("=============================================== \nConfirm adding the following item ===============================================");
                    Console.WriteLine($"Name: {name}, Category: {category}, Price: {price}, Quantity: {quantity}, Status: {status}");
                    Console.Write("Proceed? (Y/N): ");
                    string confirmation = Console.ReadLine() ?? string.Empty;

                    if (confirmation.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        using (MySqlConnection conn = new MySqlConnection(connString))
                        {
                            try
                            {
                                conn.Open();
                                string query = "INSERT INTO restaurantmenu (Name, Category, Price, Quantity, Status) VALUES (@name, @category, @price, @quantity, @status)";
                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@name", name);
                                    cmd.Parameters.AddWithValue("@category", category);
                                    cmd.Parameters.AddWithValue("@price", price);
                                    cmd.Parameters.AddWithValue("@quantity", quantity);
                                    cmd.Parameters.AddWithValue("@status", status);

                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        Console.WriteLine("\nMenu item added successfully!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nFailed to add menu item.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("\nError adding menu item: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nAdd operation canceled.");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid quantity. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid price. Please try again.");
            }
        }

        private static void UpdateMenuItem()
        {
            Console.WriteLine("=============================================== \nUpdate Menu Item ===============================================");
            Console.Write("Enter OrderID of the item to update: ");
            if (int.TryParse(Console.ReadLine(), out int orderID))
            {
                FetchMenuItem(orderID);

                Console.Write("New Name (leave blank to keep current): ");
                string newName = Console.ReadLine() ?? string.Empty;
                Console.Write("New Category (leave blank to keep current): ");
                string newCategory = Console.ReadLine() ?? string.Empty;
                Console.Write("New Price (leave blank to keep current): ");
                string newPriceInput = Console.ReadLine() ?? string.Empty;
                Console.Write("New Quantity (leave blank to keep current): ");
                string newQuantityInput = Console.ReadLine() ?? string.Empty;
                Console.Write("New Status (leave blank to keep current): ");
                string newStatus = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("=============================================== \nConfirm updating the item ===============================================");
                Console.Write("\nProceed? (Y/N): ");
                string confirmation = Console.ReadLine() ?? string.Empty;

                if (confirmation.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    using (MySqlConnection conn = new MySqlConnection(connString))
                    {
                        try
                        {
                            conn.Open();
                            string query = "UPDATE restaurantmenu SET Name = COALESCE(NULLIF(@name, ''), Name), " +
                                           "Category = COALESCE(NULLIF(@category, ''), Category), " +
                                           "Price = COALESCE(NULLIF(@price, ''), Price), " +
                                           "Quantity = COALESCE(NULLIF(@quantity, ''), Quantity), " +
                                           "Status = COALESCE(NULLIF(@status, ''), Status) WHERE OrderID = @orderID";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@name", newName);
                                cmd.Parameters.AddWithValue("@category", newCategory);
                                cmd.Parameters.AddWithValue("@price", string.IsNullOrEmpty(newPriceInput) ? (object)DBNull.Value : decimal.Parse(newPriceInput));
                                cmd.Parameters.AddWithValue("@quantity", string.IsNullOrEmpty(newQuantityInput) ? (object)DBNull.Value : int.Parse(newQuantityInput));
                                cmd.Parameters.AddWithValue("@status", newStatus);
                                cmd.Parameters.AddWithValue("@orderID", orderID);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine("\nMenu item updated successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("\nFailed to update menu item.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\nError updating menu item: " + ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nUpdate operation canceled.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid OrderID.");
            }
        }

      private static void DeleteMenuItem()
        {
            Console.WriteLine("=============================================== \nDelete Menu Item ===============================================");
            Console.Write("Enter OrderID of the item to delete: ");
            if (int.TryParse(Console.ReadLine(), out int orderID))
            {
                FetchMenuItem(orderID);

                Console.Write("\nAre you sure you want to delete this item? (Y/N): ");
                string confirmation = Console.ReadLine() ?? string.Empty;

                if (confirmation.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    using (MySqlConnection conn = new MySqlConnection(connString))
                    {
                        try
                        {
                            conn.Open();
                            string query = "DELETE FROM restaurantmenu WHERE OrderID = @orderID";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@orderID", orderID);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine("\nMenu item deleted successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("\nFailed to delete menu item.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\nError deleting menu item: " + ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nDelete operation canceled.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid OrderID.");
            }
        }

        private static void FetchMenuItem(int orderID)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM restaurantmenu WHERE OrderID = @orderID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderID", orderID);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine("\nCurrent Item Details:");
                                    Console.WriteLine($"OrderID: {reader["OrderID"]}");
                                    Console.WriteLine($"Name: {reader["Name"]}");
                                    Console.WriteLine($"Category: {reader["Category"]}");
                                    Console.WriteLine($"Price: {reader["Price"]}");
                                    Console.WriteLine($"Quantity: {reader["Quantity"]}");
                                    Console.WriteLine($"Status: {reader["Status"]}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No item found with the given OrderID.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching menu item: " + ex.Message);
                }
            }
        }

         private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Convert to hexadecimal
                }
                return builder.ToString();
            }
        }

        private static void ShowManagerMenu()
        {
            Console.WriteLine("=============================================== \nWhat would you like to do? ===============================================");
            Console.WriteLine("[1]. Delivery Personnel Assignment");
            Console.WriteLine("[2]. Restaurant Menu Database");
            Console.Write("\nAnswer: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    AssignDeliveryPersonnel();
                    break;
                case "2":
                    ManageRestaurantMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private static void FetchOrders()
{
    List<int> orderIds = new List<int>();

    using (MySqlConnection conn = new MySqlConnection(connString))
    {
        try
        {
            conn.Open();
            string query = "SELECT OrderID FROM customer_orders WHERE Status = 'Pending'";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine("=============================================== \nPending Orders ===============================================");
                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32("OrderID");
                        orderIds.Add(orderId); // Add OrderID to the list
                        Console.WriteLine($"OrderID: {orderId}");
                    }
                }
                else
                {
                    Console.WriteLine("No pending orders found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching orders: " + ex.Message);
        }
    }
}
       private static void AssignDeliveryPersonnel()
{
    Console.WriteLine("=============================================== \nAssign Delivery Personnel to Customer Order ===============================================");
    FetchOrders(); // Fetch and display pending orders

    Console.Write("Enter Customer Order ID: ");
    if (int.TryParse(Console.ReadLine(), out int orderId))
    {
        Console.Write("Enter Dispatcher ID to assign: ");
        if (int.TryParse(Console.ReadLine(), out int dispatcherId))
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE customer_orders SET DispatcherID = @dispatcherId, Status = 'Assigned' WHERE OrderID = @orderId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dispatcherId", dispatcherId);
                        cmd.Parameters.AddWithValue("@orderId", orderId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("\nDispatcher assigned successfully!");
                        }
                        else
                        {
                            Console.WriteLine("\nFailed to assign dispatcher. Please check the Order ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError assigning dispatcher: " + ex.Message);
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid Dispatcher ID.");
        }
    }
    else
    {
        Console.WriteLine("Invalid Order ID.");
    }
}

    }
}
    
