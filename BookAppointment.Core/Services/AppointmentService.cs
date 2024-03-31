using BookAppointment.Core.DTOs;
using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Common;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Core.Interfaces.Services;

namespace BookAppointment.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointment;
        private readonly IDayOffRepository _dayOff;
        private readonly IAgencyRepository _agency;
        private readonly IUnitOfWork _uow;

        public AppointmentService(IUnitOfWork uow, IAppointmentRepository appointment, IDayOffRepository dayOff, IAgencyRepository agency)
        {
            _uow=uow;
            _appointment=appointment;
            _dayOff=dayOff;
            _agency=agency;
        }

        public async Task<AppointmentResultDto> CreateAppointment(Appointment appointment)
        {
            //check day off
            if(_dayOff.GetAll().Any(x => x.DayOffDate == appointment.AppointmentDate))
            {
                throw new Exception("The selected date is not available for appointments, please choose another date.");
            }

            //check available slot
            var limitPerDay = await _appointment.GetMaximumLimit();
            DateOnly appointmentDate = appointment.AppointmentDate;
            string notes = "Booking created successfully";
            while (true)
            {
                var totalBooking = _appointment.GetAll().Count(x => x.AppointmentDate == appointmentDate);
                if (limitPerDay == 0 || totalBooking < limitPerDay)
                {
                    if (appointmentDate != appointment.AppointmentDate)
                        notes = $"The selected date has reached the maximum number of appointments. Your appointment has been automatically moved to {appointmentDate.ToString("dd-MM-yyyy")}";

                    //assign customer to next available day
                    appointment.AppointmentDate = appointmentDate;
                    break;
                }
                appointmentDate = appointmentDate.AddDays(1);
            }

            //generate token
            appointment.Token = GenerateBookingToken();

            //insert to db
            _appointment.Insert(appointment);
            await _uow.CompleteAsync();

            //return dto
            return new AppointmentResultDto
            {
                Appointment = appointment,
                Notes = notes
            };
        }

        public List<Appointment> GetAll(DateTime? dateFrom, DateTime? dateTo)
        {
            var data = _appointment.GetAppointments();
            if (dateFrom.HasValue)
            {
                if(dateFrom.Value.Date < DateTime.Now)
                    dateFrom = DateTime.Now.Date;

                data = data.Where(a => a.AppointmentDate >= DateOnly.FromDateTime(dateFrom.Value));
            }

            if (dateTo.HasValue)
            {
                data = data.Where(a => a.AppointmentDate <= DateOnly.FromDateTime(dateTo.Value));
            }

            // Order the appointments by date and time
            var queue = data.OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).ToList();

            return queue;
        }

        public async Task SetBookingLimit(int limit)
        {
            await _agency.SetLimit(limit);
        }

        private string GenerateBookingToken()
        {
            const string alphaNum = "ABCDEFGHJKLMNOPRSTUVWXYZ123456789";
            var random = new Random();  
            string token = string.Empty;
            bool isUnique;

            do
            {
                token = new string(
                    Enumerable.Repeat(alphaNum, 5)
                    .Select(token => token[random.Next(token.Length)]).ToArray());

                // check if the generated token already exists in the database
                isUnique = !_appointment.CheckExistingToken(token);

            } while (!isUnique);

            return token;
        }

    }
}
