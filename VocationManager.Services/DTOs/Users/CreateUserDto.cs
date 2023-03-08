namespace VocationManager.Services.DTOs.Users
{
    public class CreateUserDto : BaseUserDto
    {
        public CreateUserDto()
        {
            Id = "new-user";
            SelectedRole = Enum.GetName(DefaultRoles.Unassigned);
        }

        public string Password { get; set; }

        public string SelectedRole { get; set; }
    }
}
