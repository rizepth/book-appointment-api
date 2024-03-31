using BookAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Services
{
    public interface IDayOffService
    {
        Task CreateDayoff(DayOff entity);
        Task UpdateDayoff(DayOff entity);
        Task DeleteDayoff(int id);
    }
}
