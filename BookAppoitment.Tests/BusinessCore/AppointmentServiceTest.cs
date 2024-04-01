using BookAppointment.Core.DTOs;
using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Common;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Core.Services;
using Castle.DynamicProxy;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppoitment.Tests.BusinessCore
{
    public class AppointmentServiceTest
    {
        private readonly Mock<IAppointmentRepository> _mockAppointmentRepo;
        private readonly Mock<IDayOffRepository> _mockDayoffRepo;
        private readonly Mock<IAgencyRepository> _mockAgencyRepo;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly AppointmentService _sut;

        public AppointmentServiceTest()
        {
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockDayoffRepo = new Mock<IDayOffRepository>();
            _mockAgencyRepo = new Mock<IAgencyRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _sut = new AppointmentService(_mockUow.Object, _mockAppointmentRepo.Object, _mockDayoffRepo.Object, _mockAgencyRepo.Object);
        }

        [Fact]
        public async Task CreateAppointment_PostData_ReturnsDto()
        {
            //arrange
            var appointment = new Appointment()
            {
                CustomerId = 1,
                AppointmentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                AppointmentTime = new TimeSpan(12, 00, 00)
            };

            //act
            var result = await _sut.CreateAppointment(appointment);

            //assert
            result.Should().NotBeNull();
            result.Appointment.Token.Should().NotBeNull();
            result.Should().BeOfType<AppointmentResultDto>();
            result.Notes.Should().Be("Booking created successfully");
        }

        [Fact]
        public async Task CreateAppointment_MaxLimitReached_ReturnsSuccessWithNotes()
        {
            // Arrange
            var appointmentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            var appointment = new Appointment()
            {
                CustomerId = 1,
                AppointmentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                AppointmentTime = new TimeSpan(12, 00, 00)
            };

            var maxLimit = 2;
            var existingAppointments = new List<Appointment>
            {
                new Appointment { AppointmentDate = appointment.AppointmentDate },
                new Appointment { AppointmentDate = appointment.AppointmentDate },
                new Appointment { AppointmentDate = appointmentDate.AddDays(1) },
            };

            _mockAppointmentRepo.Setup(x => x.GetMaximumLimit()).ReturnsAsync(maxLimit);
            _mockAppointmentRepo.Setup(x => x.GetAll()).Returns(existingAppointments.AsQueryable());

            // Act
            var result = await _sut.CreateAppointment(appointment);

            // Assert
            result.Should().NotBeNull();
            result.Appointment.Token.Should().NotBeNull();
            result.Should().BeOfType<AppointmentResultDto>();
            result.Notes.Should().Contain("The selected date has reached the maximum number of appointments. Your appointment has been automatically moved to");
            result.Appointment.AppointmentDate.Should().BeAfter(appointmentDate);
        }

        [Fact]
        public async Task CreateAppointment_CheckDayOff_ThrowErrorWithMessage()
        {
            // Arrange
            var appointment = new Appointment()
            {
                CustomerId = 1,
                AppointmentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                AppointmentTime = new TimeSpan(12, 00, 00)
            };

            var dayOffs = new List<DayOff>
            {
                new DayOff { DayOffDate = appointment.AppointmentDate },
            };

            _mockDayoffRepo.Setup(x => x.GetAll()).Returns(dayOffs.AsQueryable());

            // Act
            // Nothing

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _sut.CreateAppointment(appointment));
            exception.Message.Should().Be("The selected date is not available for appointments, please choose another date.");
        }
    }
}
