using System;
using MySql.Data.MySqlClient;

namespace OOP
{
    class ManageProduct
    {
        public class InsertNewProduct
        {
            private string server = "localhost";
            private string database = "csharpapp";
            private string username = "root";
            private string password = "";
            private string connString;

            public InsertNewProduct()
            {
                connString = $"Server={server};Database={database};User ID={username};Password={password};";
            }

            public void InsertData(string productName, int productPrice, string productDescription)
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        Console.WriteLine("Connected to MySQL!");

                        string query = "INSERT INTO products (productName, productPrice, productDescription) VALUES (@name, @price, @description)";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", productName);
                            cmd.Parameters.AddWithValue("@price", productPrice);
                            cmd.Parameters.AddWithValue("@description", productDescription);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0) {
                                Console.WriteLine(@"
██████╗  █████╗ ████████╗ █████╗     ██╗███╗   ██╗███████╗███████╗██████╗ ████████╗███████╗██████╗     ███████╗██╗   ██╗ ██████╗ ██████╗███████╗███████╗███████╗███████╗██╗   ██╗██╗     ██╗  ██╗   ██╗
██╔══██╗██╔══██╗╚══██╔══╝██╔══██╗    ██║████╗  ██║██╔════╝██╔════╝██╔══██╗╚══██╔══╝██╔════╝██╔══██╗    ██╔════╝██║   ██║██╔════╝██╔════╝██╔════╝██╔════╝██╔════╝██╔════╝██║   ██║██║     ██║  ╚██╗ ██╔╝
██║  ██║███████║   ██║   ███████║    ██║██╔██╗ ██║███████╗█████╗  ██████╔╝   ██║   █████╗  ██║  ██║    ███████╗██║   ██║██║     ██║     █████╗  ███████╗███████╗█████╗  ██║   ██║██║     ██║   ╚████╔╝ 
██║  ██║██╔══██║   ██║   ██╔══██║    ██║██║╚██╗██║╚════██║██╔══╝  ██╔══██╗   ██║   ██╔══╝  ██║  ██║    ╚════██║██║   ██║██║     ██║     ██╔══╝  ╚════██║╚════██║██╔══╝  ██║   ██║██║     ██║    ╚██╔╝  
██████╔╝██║  ██║   ██║   ██║  ██║    ██║██║ ╚████║███████║███████╗██║  ██║   ██║   ███████╗██████╔╝    ███████║╚██████╔╝╚██████╗╚██████╗███████╗███████║███████║██║     ╚██████╔╝███████╗███████╗██║██╗
╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝    ╚═╝╚═╝  ╚═══╝╚══════╝╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═════╝     ╚══════╝ ╚═════╝  ╚═════╝ ╚═════╝╚══════╝╚══════╝╚══════╝╚═╝      ╚═════╝ ╚══════╝╚══════╝╚═╝╚═╝
                                                                                                                                                                                                       ");
                            } else {
                                Console.WriteLine(@"
███████╗ █████╗ ██╗██╗     ███████╗██████╗     ████████╗ ██████╗     ██╗███╗   ██╗███████╗███████╗██████╗ ████████╗    ██████╗  █████╗ ████████╗ █████╗    
██╔════╝██╔══██╗██║██║     ██╔════╝██╔══██╗    ╚══██╔══╝██╔═══██╗    ██║████╗  ██║██╔════╝██╔════╝██╔══██╗╚══██╔══╝    ██╔══██╗██╔══██╗╚══██╔══╝██╔══██╗   
█████╗  ███████║██║██║     █████╗  ██║  ██║       ██║   ██║   ██║    ██║██╔██╗ ██║███████╗█████╗  ██████╔╝   ██║       ██║  ██║███████║   ██║   ███████║   
██╔══╝  ██╔══██║██║██║     ██╔══╝  ██║  ██║       ██║   ██║   ██║    ██║██║╚██╗██║╚════██║██╔══╝  ██╔══██╗   ██║       ██║  ██║██╔══██║   ██║   ██╔══██║   
██║     ██║  ██║██║███████╗███████╗██████╔╝       ██║   ╚██████╔╝    ██║██║ ╚████║███████║███████╗██║  ██║   ██║       ██████╔╝██║  ██║   ██║   ██║  ██║██╗
╚═╝     ╚═╝  ╚═╝╚═╝╚══════╝╚══════╝╚═════╝        ╚═╝    ╚═════╝     ╚═╝╚═╝  ╚═══╝╚══════╝╚══════╝╚═╝  ╚═╝   ╚═╝       ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝
                                                                                                                                                           ");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}