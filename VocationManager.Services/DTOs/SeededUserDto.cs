namespace VocationManager.Services.DTOs
{
    public class SeededUserDto : BaseUserDto
    {
        public string RoleName { get; set; }

        public string Password { get; set; }
    }
}
