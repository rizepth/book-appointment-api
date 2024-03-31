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
    [Table("agency")]
    public class Agency : AuditableEntity
    {
        [MaxLength(255)]
        public string CompanyName { get; set; }
        [MaxLength(255)]
        public string EmailAddress { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(255)]
        public string PhoneNumber { get; set; }
        public int MaxAppointmentLimit { get; set; }
    }
}
