using System;
using MySql.Data.MySqlClient;

namespace User
{
    class Register
    {
        public class InsertNewData
        {
            private string server = "localhost";
            private string database = "registration";
            private string username = "root";
            private string password = "";
            private string connString;

            public InsertNewData()
            {
                connString = $"Server={server};Database={database};User ID={username};Password={password};";
            }

            public void InsertData(string wholeName, string studentNumber, int age, string cellphoneNumber)
            {
                // Use backticks (`) for column names with spaces
                string query = "INSERT INTO group10 (`Whole Name`, `Student Number`, `Age`, `Cellphone Number`) " +
                               "VALUES (@WholeName, @StudentNumber, @Age, @CellphoneNumber)";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connString))
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            // Add parameters to prevent SQL injection
                            command.Parameters.AddWithValue("@WholeName", wholeName);
                            command.Parameters.AddWithValue("@StudentNumber", studentNumber);
                            command.Parameters.AddWithValue("@Age", age);
                            command.Parameters.AddWithValue("@CellphoneNumber", cellphoneNumber);

                            // Open the connection and execute the query
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Data inserted successfully!");
                            }
                            else
                            {
                                Console.WriteLine("No rows affected. Data insertion failed.");
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Database error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}