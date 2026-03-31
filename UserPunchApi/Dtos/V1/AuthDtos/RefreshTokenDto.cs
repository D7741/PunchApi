using System.ComponentModel.DataAnnotations;

namespace UserPunchApi.Models.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}