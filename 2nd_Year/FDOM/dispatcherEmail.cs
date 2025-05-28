using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;

namespace Dispatcher.DispatcherEmail
{
    public static class DispatcherEmailOperations
    {
        private static readonly string smtpServer = "smtp.gmail.com";
        private static readonly int smtpPort = 587;
        private static readonly string senderEmail = "ativocyron@gmail.com";
        private static readonly string senderPassword = "jicb umyz dqwl xpln";

        private static readonly string connectionString = "server=localhost; database=fdom; user=root; password=;"; //Connect to the phpMyAdmin app 


        // Send Account Registration Email
        public static void DispatcherAccountRegistrationEmail()
        {
            string query = @"
                SELECT DispatcherID, Name, Email, Password, Address, ContactNumber
                FROM dispatcher
                ORDER BY DispatcherID DESC LIMIT 1";

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
                            string dispatcherName = reader["Name"].ToString() ?? string.Empty;

                            string subject = "Account Registration";
                            string body = $"Hello {dispatcherName},\n\nYour account has been successfully registered.";

                            SendDispatcherEmail(recipientEmail, subject, body);
                        }
                        else
                        {
                            Console.WriteLine("No dispatcher found in the database.");
                        }
                    }
                }
            }
        }

        // Updated SendDispatcherEmail method to accept body
        private static void SendDispatcherEmail(string recipientEmail, string subject, string body)
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

        public static void SendOrderAssignmentEmail(string toEmail, string subject, string body)
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
                Console.WriteLine($"Order assignment email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending order assignment email: {ex.Message}");
            }
        }
    }
}
