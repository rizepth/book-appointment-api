using BookAppointment.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Entities
{
    public class User : AuditableEntity
    {
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
