using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fabricam.Web.Core.DataAccess
{
    public class Lead
    {
        [Key]
        public int LeadId { get; set; }

        [Column("LeadStatusId")]
        public LeadStatus LeadStatus { get; set; }

        [StringLength(30)]
        public string Title { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(5)]
        public string Country { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }
        [StringLength(15)]
        public string Cell { get; set; }

        // TODO: clarify with users other lead properties
        // TODO: clarify with users string lengths and required fields
    }
}
