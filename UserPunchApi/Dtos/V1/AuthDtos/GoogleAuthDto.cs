using System.ComponentModel.DataAnnotations;

namespace UserPunchApi.Dtos.V1.AuthDtos
{
    public class GoogleAuthDto
    {
        [Required]
        public string Credential { get; set; } = string.Empty;
    }
}
