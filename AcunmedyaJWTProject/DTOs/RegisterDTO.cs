namespace AcunmedyaJWTProject.DTOs
{
    public class RegisterDTO
    {
        public string? Email { get; set; }
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
