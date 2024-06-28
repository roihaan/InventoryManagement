using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using InventoryWeb.Models;

public class EmailService
{
    private readonly InventoryDBContext _context;
    private readonly string _fromEmail;
    private readonly string _fromEmailPassword;
    private readonly string _managerEmail;
    private readonly string _smtpServer;

    public EmailService(InventoryDBContext context, IConfiguration configuration)
    {
        _context = context;
        _fromEmail = configuration["EmailSettings:FromEmail"];
        _fromEmailPassword = configuration["EmailSettings:FromEmailPassword"];
        _managerEmail = configuration["EmailSettings:ManagerEmail"];
        _smtpServer = configuration["EmailSettings:SmtpServer"];
    }

    public void SendDailySummary()
    {
        try
        {
            var products = _context.Products.ToList();

            // Generate HTML content
            var htmlContent = "<html><body><h1>Daily Stock Summary</h1><table border='1'><tr><th>Product ID</th><th>Name</th><th>Stock Level</th></tr>";
            foreach (var product in products)
            {
                htmlContent += $"<tr><td>{product.ProductId}</td><td>{product.Name}</td><td>{product.StockLevel}</td></tr>";
            }
            htmlContent += "</table></body></html>";

            // Send email
            SendEmail(_managerEmail, "Daily Stock Summary", htmlContent);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            Console.WriteLine(ex.Message);
        }
    }

    private void SendEmail(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = 587,
            Credentials = new NetworkCredential(_fromEmail, _fromEmailPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);

        smtpClient.Send(mailMessage);
    }
}
