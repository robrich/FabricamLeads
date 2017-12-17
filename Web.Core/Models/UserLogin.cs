using System.ComponentModel.DataAnnotations;

namespace Fabricam.Web.Core.Models
{
    // TODO: consider moving these models to a common assembly consumed by both sides

    public class UserLoginRequest
    {

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)] // max length is not important, we only store the hash
        //[RegularExpression("TODO: clarify password complexity requirements with users")]
        public string Password { get; set; }

    }

    public class UserLoginResponse : ValidationResponse
    {
        public bool UserIsValid { get; set; }
        public int UserId { get; set; }

        // TODO: include frequently used profile properties like FirstName
    }
}
