using BookAppointment.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Entities
{
    [Table("appointment")]
    public class Appointment : AuditableEntity
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateOnly AppointmentDate { get; set; }
        [Required]
        public TimeSpan AppointmentTime { get; set; }
        [MaxLength(255)]
        public string Token { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
