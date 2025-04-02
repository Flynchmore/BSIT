using System;
using System.Security.Cryptography; // For hashing
using System.Text; // For encoding strings
using System.Net.Mail; // For sending emails
using System.Net; // For email credentials
using MySql.Data.MySqlClient; // Import MySQL library
using Menu;

namespace UserOne
{
    public static class CustomerUser
    {
        private static string connString = "Server=localhost;Database=midterm;User ID=root;Password=;";

    public static void Customer()
    {
        bool exitCustomer = false;
    do
    {
        Console.WriteLine("=============================================== \nWelcome to the Customer System ===============================================");
        Console.WriteLine("[1]. Log In");
        Console.WriteLine("[2]. Register");
        Console.WriteLine("[3]. Exit");
        Console.Write("\nAnswer: ");
        string choice = Console.ReadLine() ?? string.Empty;

        switch (choice)
        {
            case "1":
                LoginCustomer();
                break;
            case "2":
                RegisterCustomer();
                break;
            case "3":
                exitCustomer = true;
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    } while (!exitCustomer);
}

        private static void RegisterCustomer()
        {
            Console.WriteLine("=============================================== \nRegister a New Account ===============================================");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            if (CheckIfCustomerExists(email))
            {
                Console.WriteLine("\nAn account with this email already exists. Please log in instead.");
                return;
            }

            // Hash the password before storing it
            string hashedPassword = HashPassword(password);

            // Insert customer data into the database
            InsertCustomer(name, hashedPassword, email);

            // Send email notification for account registration
            SendEmail(email, "Account Registration Successful\n", $"Hello {name},\n\nYour account has been successfully registered!");

            Console.Clear();
            Console.WriteLine("Account created successfully! Welcome, " + name + "!");
            Console.Write("Order now? (Y/N): ");
            string order = Console.ReadLine() ?? string.Empty;

            if (order.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                FoodMenu.Food();
            }
            else
            {
                Console.WriteLine("\nThank you for visiting! Have a great day!");
            }
        }

        private static void LoginCustomer()
        {
            Console.WriteLine("=============================================== \nLog In to Your Account ===============================================");
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;

            // Hash the entered password before checking it
            string hashedPassword = HashPassword(password);

            if (AuthenticateCustomer(email, hashedPassword))
            {
                Console.Clear();
                Console.WriteLine("=============================================== \nLogin successful! Welcome back! ===============================================");
                Console.Write("Order now? (Y/N): ");
                string order = Console.ReadLine() ?? string.Empty;

                if (order.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    FoodMenu.Food();
                }
                else
                {
                    Console.WriteLine("\nThank you for visiting! Have a great day!");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid email or password. Please try again.");
            }
        }

        private static bool CheckIfCustomerExists(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM customer WHERE Email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error checking customer existence: " + ex.Message);
                    return false;
                }
            }
        }

        private static bool AuthenticateCustomer(string email, string hashedPassword)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM customer WHERE Email = @Email AND Password = @Password";
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
                    Console.WriteLine("Error authenticating customer: " + ex.Message);
                    return false;
                }
            }
        }

        private static void InsertCustomer(string name, string hashedPassword, string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connected to the database!");

                    string query = "INSERT INTO customer (Name, Password, Email) VALUES (@name, @password, @Email)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Customer data inserted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to insert customer data.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

    public class Order
    {
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public string Status { get; set; } 
    public Order()
         {
         Status = "Pending";
         }
    public DateTime OrderDate { get; set; } 
    }
   private static void ViewOrders(int customerId)
{
    Console.WriteLine("Fetching your orders...");

    using (MySqlConnection conn = new MySqlConnection(connString))
    {
        try
        {
            conn.Open();
            string query = "SELECT OrderID, Status, OrderDate FROM customer_orders WHERE CustomerID = @CustomerID";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Order> orders = new List<Order>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderID = reader.GetInt32("OrderID"),
                                Status = reader["Status"]?.ToString() ?? "Pending", // Use database value or default
                                OrderDate = reader["OrderDate"] != DBNull.Value 
                                    ? reader.GetDateTime("OrderDate") 
                                    : DateTime.Now // Fallback value if OrderDate is NULL
                            });
                        }

                        Console.WriteLine("=============================================== \nYour Orders ===============================================");
                        foreach (var order in orders)
                        {
                            Console.WriteLine($"Order ID: {order.OrderID}");
                            Console.WriteLine($"Status: {order.Status}");
                            Console.WriteLine($"Order Date: {order.OrderDate:yyyy-MM-dd}");
                            Console.WriteLine("------------------------------------------------");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have no orders.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching orders: " + ex.Message);
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

        private static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("ativocyron@gmail.com"); // Replace with your email
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)) // Replace with your SMTP server
                    {
                        smtp.Credentials = new NetworkCredential("ativocyron@gmail.com", "@cC3LeR@t3at_21"); // Replace with your email credentials
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                Console.WriteLine("Email sent successfully to " + toEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}