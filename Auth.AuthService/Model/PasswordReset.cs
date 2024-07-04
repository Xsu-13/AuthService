using Auth.AuthService.Entity;

namespace Auth.AuthService.Model
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public bool? isUsed { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiredAt { get; set; } = DateTime.UtcNow.AddHours(24);
        public virtual UserEntity? User { get; set; }
        public int UserId { get; set; }
    }
}
