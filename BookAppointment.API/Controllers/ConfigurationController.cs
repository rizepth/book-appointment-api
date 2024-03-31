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


        [HttpPatch("set-max-limit")]
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

        [HttpPost("create-dayoff")]
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

        [HttpPut("update-dayoff/{id:int}")]
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

        [HttpDelete("delete-dayoff/{id:int}")]
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
