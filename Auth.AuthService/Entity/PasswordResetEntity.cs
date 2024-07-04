namespace Auth.AuthService.Entity
{
    public class PasswordResetEntity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public bool? isUsed { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiredAt { get; set; } = DateTime.UtcNow.AddHours(24);
        public virtual UserEntity? User { get; set; }
        public int UserId { get; set; }
    }
}
