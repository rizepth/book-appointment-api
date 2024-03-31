using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Entities.Common
{
    public abstract class AuditableEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [MaxLength(255)]
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [MaxLength(255)]
        public string? UpdatedBy { get; set; }
    }
}
