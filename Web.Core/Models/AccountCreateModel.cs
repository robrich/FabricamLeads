using System.ComponentModel.DataAnnotations;

namespace Fabricam.Web.Core.Models
{
    public class AccountCreateModel
    {

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)] // max length is not important, we'll only store the hash
        //[RegularExpression("TODO: clarify password complexity requirements with users")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
