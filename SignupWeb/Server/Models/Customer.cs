using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SignupWeb.Server.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public long? ID { get; set; }

        public long? CardNumber { get; set; }

        [MaxLength(30)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }

        [MaxLength(10)]
        public string? PostCode { get; set; }

        [MaxLength(12)]
        public string? Phone { get; set; }

        [MaxLength(12)]
        public string? Mobile { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        public DateTime? DOB { get; set; }

        [MaxLength(10)]
        public string? Password { get; set; }
    }
}
