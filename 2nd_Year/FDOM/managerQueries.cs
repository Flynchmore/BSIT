using System;
using MySql.Data.MySqlClient;

namespace Manager.ManagerQueries {
public class ManagerDB
{
    private static string connectionString = "Server=localhost;Database=fdom;Uid=root;Pwd=@cC3LeR@t3at_21;"; //Connect to the MySQL Stand alone app 

    //For the Manager Account Registration
   public static void managerRegister(string name, string email, string password, string address, string contactNumber)
    {
        string query = $@"
            INSERT INTO manager (Name, Email, Password, Address, `Contact Number`)
            VALUES (@Name, @Email, @Password, @Address, @ContactNumber);";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@ContactNumber", contactNumber);

                connection.Open();
                int rowsInserted = command.ExecuteNonQuery();
                Console.WriteLine(rowsInserted > 0 ? $"Manager account registered successfully!" : $"Manager account registration failed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }

    public static bool ValidateManagerCredentials(string email, string password)
    {
        string query = @"
            SELECT COUNT(*)
            FROM manager
            WHERE Email = @Email AND Password = @Password";

        var parameters = new Dictionary<string, object>
        {
            { "@Email", email },
            { "@Password", password },
        };

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0; // Return true if a matching user is found
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
            return false;
        }
    }

    // Manager functionality to add menu items
    public static void AddMenuItem(string name, decimal price, int category, int stacks)
    {
        string query = @"INSERT INTO food_menu (Name, Price, Category, Stacks) VALUES (@Name, @Price, @Category, @Stacks);";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Stacks", stacks);

                connection.Open();
                int rowsInserted = command.ExecuteNonQuery();
                Console.WriteLine(rowsInserted > 0 ? "Food Menu item added successfully!" : "Failed to add food menu item.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while adding food menu item: {ex.Message}");
        }
    }

    // Manager functionality to update menu items
    public static void UpdateMenuItem(int id, string name, decimal price, int category, int stacks)
    {
        string query = @"UPDATE food_menu SET Name = @Name, Price = @Price, Category = @Category, Stacks = @Stacks WHERE FoodID = @FoodID;";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FoodID", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Stacks", stacks);

                connection.Open();
                int rowsUpdated = command.ExecuteNonQuery();
                Console.WriteLine(rowsUpdated > 0 ? "Food Menu item updated successfully!" : "Failed to update food menu item.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating food menu item: {ex.Message}");
        }
    }

    //Manager functionality to delete menu items
    public static void DeleteMenuItem(int id)
    {
        string query = @"DELETE FROM food_menu WHERE FoodID = @FoodID;";
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FoodID", id);

                connection.Open();
                int rowsDeleted = command.ExecuteNonQuery();
                Console.WriteLine(rowsDeleted > 0 ? "Food Menu item deleted successfully!" : "Failed to delete food menu item.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting menu item: {ex.Message}");
        }
    }

    //Manager functionality to read menu items
    public static void ReadMenuItems()
    {
        string query = @"SELECT * FROM food_menu;";
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nCurrent Food Menu Items:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"FoodID: {reader["FoodID"]}, Name: {reader["Name"]}, Price: {reader["Price"]}, Category: {reader["Category"]}, Stacks: {reader["Stacks"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading menu items: {ex.Message}");
        }
    }
}
}
