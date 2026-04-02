using System.ComponentModel.DataAnnotations;

namespace UserPunchApi.Dtos.V1.AuthDtos

{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}