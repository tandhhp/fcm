namespace Waffle.Models.Args
{
    public class SendEmailsArgs
    {
        public List<string>? UserIds { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
