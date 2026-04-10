namespace UserPunchApi.Dtos.V1.UserDtos
{
    public class UserResponseDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";
        public string Role { get; set; } = "Employee";

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public long? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }
}