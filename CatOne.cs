using System;
using MySql.Data.MySqlClient; // Import MySQL library

namespace CatOne
{
    public static class CatBreakfast
    {
        private static string connString = "Server=localhost;Database=midterm;User ID=root;Password=;";

        public static void Breakfast()
        {
            Console.WriteLine("=============================================== \nWelcome to the Breakfast Menu! ===============================================");

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Fetching Breakfast Menu from the database...");

                    string query = "SELECT Name, Quantity, Price, Status FROM restaurantmenu WHERE Category = 'Breakfast'";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("=============================================== \nAvailable Breakfast Items ===============================================");
                        Console.WriteLine("===========================================================================================================================");

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string name = reader.GetString("Name");
                                int quantity = reader.GetInt32("Quantity");
                                decimal price = reader.GetDecimal("Price");
                                string status = reader.GetString("Status");

                                Console.WriteLine($"- {name}");
                                Console.WriteLine($"  Quantity: {quantity}");
                                Console.WriteLine($"  Price: ${price}");
                                Console.WriteLine($"  Status: {status}");
                                Console.WriteLine("=====================================================================================================================");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nNo breakfast items available in the menu.");
                        }
                    }

                    // Allow the customer to pick an order
                    PickOrder(conn);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError fetching breakfast menu: " + ex.Message);
                }
            }
        }

        private static void PickOrder(MySqlConnection conn)
        {
            Console.Write("\nEnter the name of the item you want to order:");
            string itemName = Console.ReadLine() ?? string.Empty;

            string query = "SELECT Name, Price, Quantity, Status FROM restaurantmenu WHERE Name = @itemName AND Category = 'Breakfast'";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@itemName", itemName);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        string name = reader.GetString("Name");
                        decimal price = reader.GetDecimal("Price");
                        int quantity = reader.GetInt32("Quantity");
                        string status = reader.GetString("Status");

                        if (status.Equals("Available", StringComparison.CurrentCultureIgnoreCase) && quantity > 0)
                        {
                            Console.WriteLine("===========================================================================================================================");
                            Console.WriteLine("=============================================== \nOrder Summary ===========================================================");
                            Console.WriteLine($"Item: {name}");
                            Console.WriteLine($"Price: ${price}");
                            Console.Write("Confirm your order? (Y/N): ");
                            string confirmation = Console.ReadLine() ?? string.Empty;
                            if (confirmation.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                PlaceOrder(conn, name, price);
                            }
                            else
                            {
                                Console.WriteLine("Order canceled.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Sorry, this item is currently unavailable.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Item not found in the menu.");
                    }
                    Console.WriteLine("==================================================================================================================================");
                }
            }
        }

        private static void PlaceOrder(MySqlConnection conn, string itemName, decimal price)
        {
            Console.Write("Enter your Customer ID: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                string query = "INSERT INTO customer_orders (CustomerID, OrderDetails, Status) VALUES (@customerId, @orderDetails, 'Pending')";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@orderDetails", $"{itemName} - ${price}");

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Order placed successfully!");
                        GenerateReceipt(conn, customerId);
                    }
                    else
                    {
                        Console.WriteLine("Failed to place the order.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Customer ID.");
            }
        }

        private static void GenerateReceipt(MySqlConnection conn, int customerId)
        {
            Console.WriteLine("=============================================== \nGenerating your receipt ===============================================");

            string query = "SELECT OrderID, OrderDetails, Status FROM customer_orders WHERE CustomerID = @customerId ORDER BY OrderID DESC LIMIT 1";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@customerId", customerId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        int orderId = reader.GetInt32("OrderID");
                        string orderDetails = reader.GetString("OrderDetails");
                        string status = reader.GetString("Status");
                        Console.WriteLine("=============================================== \nReceipt ===========================================================");
                        Console.WriteLine($"Order ID: {orderId}");
                        Console.WriteLine($"Order Details: {orderDetails}");
                        Console.WriteLine($"Status: {status}");
                        Console.WriteLine("=====================================================================================================================");

                        TrackOrder(conn, orderId);
                    }
                    else
                    {
                        Console.WriteLine("No recent orders found.");
                    }
                }
            }
        }

        private static void TrackOrder(MySqlConnection conn, int orderId)
        {
            Console.WriteLine("=============================================== \nTracking your order ===============================================");
            string query = "SELECT Status FROM customer_orders WHERE OrderID = @orderId";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@orderId", orderId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        string status = reader.GetString("Status");
                        Console.WriteLine($"Current Order Status: {status}");

                        if (status.Equals("Assigned", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Console.WriteLine("Your order has been assigned to a delivery personnel.");
                        }
                        else if (status.Equals("Delivered", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Console.WriteLine("Your order has been delivered. Thank you!");
                        }
                        else
                        {
                            Console.WriteLine("Your order is still pending.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Order not found.");
                    }
                }
            }
        }
    }
}