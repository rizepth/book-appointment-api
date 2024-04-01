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

        /// <summary>
        /// Register a new customer.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a new customer to register by providing their username, fullname, password, and other necessary information.
        /// </remarks>
        /// <param name="request">The customer registration request.</param>
        /// <returns>Returns the message if the customer successfully registered.</returns>
        [HttpPost("customer/register")]
        [ProducesResponseType(typeof(SuccessResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Customer login.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a registered customer to log in using their username and password.
        /// 
        /// For try initial process, please use this user :
        /// `username : customer1`
        /// `password : 123456` 
        /// </remarks>
        /// <param name="request">The customer login request.</param>
        /// <returns>Returns the authentication JWT token upon successful login.</returns>
        [HttpPost("customer/login")]
        [ProducesResponseType(typeof(SuccessResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Register a new agency user.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a new agency user to register by providing their username and password.
        /// </remarks>
        /// <param name="request">The agency user registration request.</param>
        /// <returns>Returns the message if the agency user successfully registered.</returns>
        [HttpPost("agency/register")]
        [ProducesResponseType(typeof(SuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Agency user login.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a registered agency user to log in using their username and password.
        /// 
        /// For try initial process, please use this user :
        /// `username : admin`
        /// `password : 123456`
        /// </remarks>
        /// <param name="request">The agency user login request.</param>
        /// <returns>Returns the authentication JWT token upon successful login.</returns>
        [HttpPost("agency/login")]
        [ProducesResponseType(typeof(SuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
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
