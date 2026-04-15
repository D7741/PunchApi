using System.ComponentModel.DataAnnotations;
using UserPunchApi.Common;

namespace UserPunchApi.Dtos.V1.UserDtos
{
    public class CreateUserDto
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [AllowedValues(Roles.Manager, Roles.Employee,
            ErrorMessage = "Role must be 'Manager' or 'Employee'.")]
        public string Role { get; set; } = Roles.Employee;

        public bool IsActive { get; set; } = true;

        public long? DepartmentId { get; set; }
    }
}
