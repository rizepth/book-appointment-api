using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Entities
{
    [Table("customer")]
    public class Customer : User
    {
        [Required]
        [MaxLength(50)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
    }
}
