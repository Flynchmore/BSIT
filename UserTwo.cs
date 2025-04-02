using System;
using System.Security.Cryptography; // For hashing
using System.Text; // For encoding strings
using System.Net.Mail; // For sending emails
using System.Net; // For email credentials
using MySql.Data.MySqlClient; // Import MySQL library

namespace UserTwo
{
    public static class DispatcherUser
    {
        private static string connString = "Server=localhost;Database=midterm;User ID=root;Password=;";

        public static void Dispatcher()
        {
            Console.WriteLine("=============================================== \nWelcome to the Dispatcher System! ===============================================");
            Console.Write("Do you already have an account? (Y/N): ");
            string hasAccount = Console.ReadLine() ?? string.Empty;

            if (hasAccount.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                LoginDispatcher();
            }
            else
            {
                RegisterDispatcher();
            }
        }


        private static void RegisterDispatcher()
        {
            Console.WriteLine("=============================================== \nRegister a New Account ===============================================");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            if (CheckIfDispatcherExists(email))
            {
                Console.WriteLine("\nAn account with this email already exists. Please log in instead.");
                return;
            }

            // Hash the password before storing it
            string hashedPassword = HashPassword(password);

            // Insert dispatcher data into the database
            InsertDispatcher(name, hashedPassword, email);

            // Send email notification for account registration
            SendEmail(email, "Account Registration Successful\n", $"Hello {name},\n\nYour dispatcher account has been successfully registered!");

            Console.Clear();
            Console.WriteLine("=============================================== \nAccount created successfully! Welcome, " + name + "! ===============================================");
            Console.Write("Check Order Status Updates? (Y/N): ");
            string order = Console.ReadLine() ?? string.Empty;

            if (order.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                CheckOrderStatus();
            }
            else
            {
                Console.WriteLine("\nThank you for visiting! Have a great day!");
            }
        }

        private static void LoginDispatcher()
        {
            Console.WriteLine("=============================================== \nLog In to Your Account ===============================================");
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;

            // Hash the entered password before checking it
            string hashedPassword = HashPassword(password);

            if (AuthenticateDispatcher(email, hashedPassword))
            {
                Console.WriteLine("=============================================== \nLogin successful! Welcome back! ===============================================");
                Console.Write("Check Order Status Updates? (Y/N): ");
                string order = Console.ReadLine() ?? string.Empty;

                if (order.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    CheckOrderStatus();
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

        private static bool CheckIfDispatcherExists(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM dispatcher WHERE Email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error checking dispatcher existence: " + ex.Message);
                    return false;
                }
            }
        }

        private static bool AuthenticateDispatcher(string email, string hashedPassword)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM dispatcher WHERE Email = @Email AND Password = @Password";
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
                    Console.WriteLine("Error authenticating dispatcher: " + ex.Message);
                    return false;
                }
            }
        }

        private static void InsertDispatcher(string name, string hashedPassword, string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connected to the database!");

                    string query = "INSERT INTO dispatcher (Name, Password, Email) VALUES (@name, @password, @Email)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Dispatcher data inserted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to insert dispatcher data.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

private static List<Order> FetchAssignedOrders(int dispatcherId)
{
    List<Order> assignedOrders = new List<Order>();

    using (MySqlConnection conn = new MySqlConnection(connString))
    {
        try
        {
            conn.Open();
            string query = "SELECT OrderID, CustomerID, Status FROM customer_orders WHERE DispatcherID = @dispatcherId AND Status = 'Assigned'";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@dispatcherId", dispatcherId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            assignedOrders.Add(new Order
                            {
                                OrderID = reader.GetInt32("OrderID"),
                                CustomerID = reader.GetInt32("CustomerID"),
                                Status = reader.GetString("Status")
                            });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No assigned orders found.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching assigned orders: " + ex.Message);
        }
    }

    return assignedOrders;
}
   private static void CheckOrderStatus()
{
    Console.WriteLine("=============================================== \nChecking Assigned Orders ===============================================");
    Console.Write("Enter your Dispatcher ID: ");
    if (int.TryParse(Console.ReadLine(), out int dispatcherId))
    {
        List<Order> assignedOrders = FetchAssignedOrders(dispatcherId);

        if (assignedOrders.Count > 0)
        {
            Console.WriteLine("=============================================== \nAssigned Orders ===============================================");
            foreach (var order in assignedOrders)
            {
                Console.WriteLine($"OrderID: {order.OrderID}, CustomerID: {order.CustomerID}, Status: {order.Status}");
            }
        }
        else
        {
            Console.WriteLine("No assigned orders found.");
        }
    }
    else
    {
        Console.WriteLine("Invalid Dispatcher ID.");
    }
}

    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string Status { get; set; }

         // Constructor with default value for Status
         public Order()
         {
         Status = "Pending";
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