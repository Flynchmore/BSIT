using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;

namespace Manager.ManagerEmail
{
    public static class ManagerEmailOperations
    {
        private static readonly string smtpServer = "smtp.gmail.com";
        private static readonly int smtpPort = 587;
        private static readonly string senderEmail = "ativocyron@gmail.com";
        private static readonly string senderPassword = "jicb umyz dqwl xpln";

        private static readonly string connectionString = "Server=localhost;Database=fdom;Uid=root;Pwd=@cC3LeR@t3at_21;";

        // Send Account Registration Email
        public static void ManagerAccountRegistrationEmail()
{
    string query = @"
        SELECT ManagerID, Name, Email, Password, Address, `Contact Number`
        FROM manager
        ORDER BY ManagerID DESC LIMIT 1";

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
                    string managerName = reader["Name"].ToString() ?? string.Empty;

                    string subject = "Account Registration";
                    string body = $"Hello {managerName},\n\nYour account has been successfully registered.";

                    SendManagerEmail(recipientEmail, subject, body);
                }
                else
                {
                    Console.WriteLine("No manager found in the database.");
                }
            }
        }
    }
}

// Updated SendManagerEmail method to accept body
private static void SendManagerEmail(string recipientEmail, string subject, string body)
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

    }
}