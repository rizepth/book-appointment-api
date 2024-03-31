﻿using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Infrastructure.Repository
{
    public class DayOffRepository : BaseRepository<DayOff>, IDayOffRepository
    {
        public DayOffRepository(DataContext context) : base(context)
        {
        }
    }
}
