using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Common;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Services
{
    public class DayoffService : IDayOffService
    {
        private readonly IDayOffRepository _dayoff;
        private readonly IUnitOfWork _uow;
        public DayoffService(IDayOffRepository dayoff, IUnitOfWork uow)
        {
            _dayoff=dayoff;
            _uow=uow;
        }

        public async Task CreateDayoff(DayOff entity)
        {
            _dayoff.Insert(entity);
            await _uow.CompleteAsync();
        }

        public async Task UpdateDayoff(DayOff entity)
        {
            var data = await _dayoff.GetById(entity.Id);
            if (data == null)
            {
                throw new Exception("Data not found");
            }

            data.DayOffDate = entity.DayOffDate;
            data.DayOffNotes = entity.DayOffNotes;
            _dayoff.Update(data);
            await _uow.CompleteAsync();
        }

        public async Task DeleteDayoff(int id)
        {
            var data = await _dayoff.GetById(id);
            if(data == null)
            {
                throw new Exception("Data not found");
            }
            _dayoff.Delete(data);
            await _uow.CompleteAsync();
        }
    }
}
