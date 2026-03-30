using UserPunchApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserPunchApi.Models
{
    public class Department{

        [Key]
        [Required]
        public int DepartmentId {get; set;}

        [Required]
        public string DepartmentName {get; set;} = string.Empty;

        public List<User> Users {get; set;} = new();

    }

    
}