using System;
using MySql.Data.MySqlClient;

namespace Dispatcher.DispatcherQueries {
public class DispatcherDB
{
    private static string connectionString = "Server=localhost;Database=fdom;Uid=root;Pwd=@cC3LeR@t3at_21;"; //Connect to the MySQL Stand alone app 

    //For the Dispatcher Account Registration
   public static void dispatcherRegister(string name, string email, string password, string address, string contactNumber)
    {
        string query = $@"
            INSERT INTO dispatcher (Name, Email, Password, Address, `Contact Number`)
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
                Console.WriteLine(rowsInserted > 0 ? $"Dispatcher account registered successfully!" : $"Dispatcher account registration failed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }

    public static bool ValidateDispatcherCredentials(string email, string password)
    {
        string query = @"
            SELECT COUNT(*)
            FROM dispatcher
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
    }
    }