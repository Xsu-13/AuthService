namespace Auth.AuthService.Model
{
    public class User(string name, string email, string password)
    {
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}
