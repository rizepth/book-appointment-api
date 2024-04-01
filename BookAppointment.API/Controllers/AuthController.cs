using BookAppointment.API.DTOs.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookAppointment.Core.Interfaces.Services;
using AutoMapper;
using BookAppointment.Core.Entities;
using BookAppointment.API.Authentication;
using BookAppointment.API.DTOs.Response.Common;

namespace BookAppointment.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerService _customer;
        private readonly IAgencyUserService _agency;
        private readonly IMapper _mapper;
        private readonly JwtGenerator _jwtGenerator;
        public AuthController(ICustomerService customer, IMapper mapper, IAgencyUserService agency, JwtGenerator jwtGenerator)
        {
            _customer=customer;
            _mapper=mapper;
            _agency=agency;
            _jwtGenerator=jwtGenerator;
        }

        [HttpPost("customer/register")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerRequest req)
        {
            var customer = _mapper.Map<Customer>(req);
            try
            {
                await _customer.Register(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message.ToString()));
            }

            return Ok(new SuccessResponse<object>(message:"Customer registered successfully"));
        }

        [HttpPost("customer/login")]
        public async Task<IActionResult> LoginCustomer(LoginRequest req)
        {
            var authUserTask = _customer.Authenticate(req.Username, req.Password);
            var authUser = await authUserTask;

            if (authUser  == null)
            {
                return Unauthorized(new ErrorResponse("Invalid username or password"));
            }

            var token = _jwtGenerator.GenerateToken(authUser, UserType.Customer);
            return Ok(new SuccessResponse<string>(token));
        }

        [HttpPost("agency/register")]
        public async Task<IActionResult> RegisterAgency(RegisterRequest req)
        {
            var user = _mapper.Map<AgencyUser>(req);
            try
            {
                await _agency.Register(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message.ToString()));
            }

            return Ok(new SuccessResponse<object>(message: "Agency user registered successfully"));
        }

        [HttpPost("agency/login")]
        public async Task<IActionResult> LoginAgency(LoginRequest req)
        {
            var authUserTask = _agency.Authenticate(req.Username, req.Password);
            var authUser = await authUserTask;

            if (authUser  == null)
            {
                return Unauthorized(new ErrorResponse("Invalid username or password"));
            }

            var token = _jwtGenerator.GenerateToken(authUser, UserType.Agency);
            return Ok(new SuccessResponse<string>(token));
        }
    }
}
