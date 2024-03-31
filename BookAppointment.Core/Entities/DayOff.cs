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
    [Table("dayoff")]
    public class DayOff : AuditableEntity
    {
        [Required]
        public DateOnly DayOffDate { get; set; }
        [Required]
        [MaxLength(255)]
        public string DayOffNotes { get; set; }
    }
}
