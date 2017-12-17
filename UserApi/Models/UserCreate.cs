using System.ComponentModel.DataAnnotations;

namespace Fabricam.UserApi.Models
{
    // TODO: consider moving these models to a common assembly consumed by both sides

    public class UserCreateRequest
    {

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)] // max length is not important, we'll only store the hash
        //[RegularExpression("TODO: clarify password complexity requirements with users")]
        public string Password { get; set; }

        // TODO: include profile properties like FirstName, LastName, Phone, etc

    }

    public class UserCreateResponse : ValidationResponse
    {
        public int UserId { get; set; }
    }
}
