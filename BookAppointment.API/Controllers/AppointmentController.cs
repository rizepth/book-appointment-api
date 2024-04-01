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

        /// <summary>
        /// Book a new appointment.
        /// </summary>
        /// <remarks>
        /// To book a new appointment, the customer needs to provide their details such as name, email, phone number,
        /// along with the desired appointment date and time.
        ///
        /// The API will check for the availability of the selected date and time slot.
        /// If the selected date is an off day (public holiday), an error response will be returned.
        /// If the maximum number of appointments per day is reached, the customer will be automatically assigned to the next available day.
        ///
        /// Upon successful booking, a unique booking token will be generated and returned in the response.
        ///
        /// Sample request:
        ///
        /// POST /api/appointment/booking
        /// {
        ///     "appointmentDate": "2023-06-10",
        ///     "appointmentTime": "10:00"
        /// }
        /// </remarks>
        /// <param name="request">The appointment booking request.</param>
        /// <returns>Returns the booked appointment details along with the booking token.</returns>
        [Authorize(Roles = UserType.Customer)]
        [HttpPost("booking")]
        [ProducesResponseType(typeof(SuccessResponse<BookAppointmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Get the queue of customers with appointments.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the agency to view the queue of customers with appointments for a specific day.
        ///
        /// The date range parameters `dateFrom` and `dateTo` are optional. If not provided, the API will return all queue.
        ///
        /// Sample request:
        ///
        /// GET /api/appointment/view-queue?dateFrom=2023-06-10&amp;dateTo=2023-06-10
        /// </remarks>
        /// <param name="dateFrom">The start date of the date range (optional).</param>
        /// <param name="dateTo">The end date of the date range (optional).</param>
        /// <returns>Returns the queue of customers with appointments for the specified date range.</returns>
        [Authorize(Roles = UserType.Agency)]
        [HttpGet("view-queue")]
        [ProducesResponseType(typeof(SuccessResponse<QueueAppointmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
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
