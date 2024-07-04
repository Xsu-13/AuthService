namespace Auth.MailService.Contracts.ConsumerContracts
{
    public record MailConfirmationRequest(string mail, string link)
    {
        string Mail { get; set; } = mail;

        string Link { get; set; } = link;
    }
}
