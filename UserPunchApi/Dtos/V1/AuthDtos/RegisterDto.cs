using System.ComponentModel.DataAnnotations;
using UserPunchApi.Common;

namespace UserPunchApi.Dtos.V1.AuthDtos
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
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
    }
}