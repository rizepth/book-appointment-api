using AutoMapper;
using BookAppointment.API.Authentication;
using BookAppointment.API.DTOs.Request;
using BookAppointment.API.DTOs.Response;
using BookAppointment.API.DTOs.Response.Common;
using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookAppointment.API.Controllers
{
    [Authorize]
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointment;
        public AppointmentController(IMapper mapper, IAppointmentService appointment)
        {
            _mapper=mapper;
            _appointment=appointment;
        }

        [Authorize(Roles = UserType.Customer)]
        [HttpPost("booking")]
        public async Task<IActionResult> BookDate(BookAppointmentRequest req)
        {
            var data = _mapper.Map<Appointment>(req);
            try
            {
                data.CustomerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response = await _appointment.CreateAppointment(data);
                string notes = response.Notes;

                return Ok(new SuccessResponse<BookAppointmentResponse>(_mapper.Map<BookAppointmentResponse>(response.Appointment), notes));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message.ToString()));
            }

        }

        [Authorize(Roles = UserType.Agency)]
        [HttpGet("view-queue")]
        public IActionResult ViewQueue(DateTime? dateFrom, DateTime? dateTo)
        {
            var data = _appointment.GetAll(dateFrom, dateTo);
            var response = new QueueAppointmentResponse()
            {
                TotalAppointments = data.Count(),
                Appointments = _mapper.Map<List<AppointmentView>>(data)
            };
            return Ok(new SuccessResponse<QueueAppointmentResponse>(response));
        }
    }
}
