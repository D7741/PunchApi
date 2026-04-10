namespace UserPunchApi.Dtos.V1.UserDtos
{
    public class CreateUserDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Employee";
        public bool IsActive { get; set; } = true;
        public long? DepartmentId { get; set; }
    }
}