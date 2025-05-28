using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;

namespace Customer.CustomerEmail
{
    public static class CustomerEmailOperations
    {
        private static readonly string smtpServer = "smtp.gmail.com";
        private static readonly int smtpPort = 587;
        private static readonly string senderEmail = "ativocyron@gmail.com";
        private static readonly string senderPassword = "jicb umyz dqwl xpln";

        private static readonly string connectionString = "server=localhost; database=fdom; user=root; password=;"; //Connect to the phpMyAdmin app 


        // Send Account Registration Email
        public static void CustomerAccountRegistrationEmail()
        {
            string query = @"
            SELECT CustomerID, Name, Email, Password, Address, ContactNumber
            FROM customer
            ORDER BY CustomerID DESC LIMIT 1";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string recipientEmail = reader["Email"].ToString() ?? string.Empty;
                            string customerName = reader["Name"].ToString() ?? string.Empty;

                            string subject = "Account Registration";
                            string body = $"Hello {customerName},\n\nYour account has been successfully registered.";

                            SendCustomerEmail(recipientEmail, subject, body);
                        }
                        else
                        {
                            Console.WriteLine("No customer found in the database.");
                        }
                    }
                }
            }
        }

        // Updated SendCustomerEmail method to accept body
        private static void SendCustomerEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient(smtpServer);

                mail.From = new MailAddress(senderEmail);
                mail.To.Add(recipientEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;

                smtp.Port = smtpPort;
                smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtp.EnableSsl = true;

                smtp.Send(mail);
                Console.WriteLine($"Email sent successfully to {recipientEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public static void SendOrderReceiptEmail(string toEmail, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient(smtpServer);

                mail.From = new MailAddress(senderEmail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;

                smtp.Port = smtpPort;
                smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtp.EnableSsl = true;

                smtp.Send(mail);
                Console.WriteLine($"Order receipt email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending order receipt email: {ex.Message}");
            }
        }
    }
}
