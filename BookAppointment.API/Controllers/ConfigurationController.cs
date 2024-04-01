using BookAppointment.API.Authentication;
using BookAppointment.API.DTOs.Response.Common;
using BookAppointment.API.DTOs.Response;
using BookAppointment.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookAppointment.Core.Entities;
using BookAppointment.API.DTOs.Request;
using AutoMapper;

namespace BookAppointment.API.Controllers
{
    [Authorize(Roles = UserType.Agency)]
    [Route("api/configuration")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IAppointmentService _appointment;
        private readonly IDayOffService _dayoff;
        private readonly IMapper _mapper;

        public ConfigurationController(IAppointmentService appointment, IMapper mapper, IDayOffService dayoff)
        {
            _appointment=appointment;
            _mapper=mapper;
            _dayoff=dayoff;
        }

        /// <summary>
        /// Set the maximum number of appointments per day.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the agency to set the maximum number of appointments allowed per day.
        /// </remarks>
        /// <param name="request">The request containing the maximum number of appointments per day.</param>
        /// <returns>Returns a success message upon setting the maximum limit.</returns>
        [HttpPatch("set-max-limit")]
        [ProducesResponseType(typeof(SuccessResponse<Object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetMaximumLimit(int maximumLimit)
        {
            try
            {
                await _appointment.SetBookingLimit(maximumLimit);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message.ToString()));
            }
            return Ok(new SuccessResponse<Object>());
        }

        /// <summary>
        /// Create a new day off.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the agency to create a new day off e.g (public holiday) on which appointments cannot be booked.
        /// </remarks>
        /// <param name="request">The request containing the day off details.</param>
        /// <returns>Returns a success message.</returns>
        [HttpPost("create-dayoff")]
        [ProducesResponseType(typeof(SuccessResponse<Object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateDayOff(DayoffRequest req)
        {
            var data = _mapper.Map<DayOff>(req);
            try
            {
                await _dayoff.CreateDayoff(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message.ToString()));
            }
            return Ok(new SuccessResponse<object>());
        }

        /// <summary>
        /// Update an existing day off.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the agency to update the details of an existing day off.
        /// </remarks>
        /// <param name="request">The request containing the updated off day details.</param>
        /// <returns>Returns a success message.</returns>
        [HttpPut("update-dayoff/{id:int}")]
        [ProducesResponseType(typeof(SuccessResponse<Object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDayOff(DayoffRequest req, int id)
        {
            var data = _mapper.Map<DayOff>(req);
            data.Id = id;
            try
            {
                await _dayoff.UpdateDayoff(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message.ToString()));
            }
            return Ok(new SuccessResponse<object>());
        }

        /// <summary>
        /// Delete a day off.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the agency to delete an existing day off.
        /// </remarks>
        /// <param name="id">The ID of the day off to delete.</param>
        /// <returns>Returns a success message upon deleting the day off.</returns>
        [HttpDelete("delete-dayoff/{id:int}")]
        [ProducesResponseType(typeof(SuccessResponse<Object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDayOff(int id)
        {
            try
            {
                await _dayoff.DeleteDayoff(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message.ToString()));
            }
            return Ok(new SuccessResponse<object>());
        }
    }
}
