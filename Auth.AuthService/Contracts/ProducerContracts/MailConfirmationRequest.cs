namespace Auth.AuthService.Contracts.ProducerContracts
{
    public record MailConfirmationRequest(string mail, string link)
    {
        string Mail { get; set; } = mail;

        string Link { get; set; } = link;
    }
}
