namespace MailService.Models;

public class EmailRequest
{
    public string Name { get; set; }
    public string Mail { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
}