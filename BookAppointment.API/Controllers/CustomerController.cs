using BookAppointment.API.Authentication;
using BookAppointment.API.DTOs.Response.Common;
using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookAppointment.API.Controllers
{
    [Authorize(Roles = UserType.Agency)]
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customer;
        public CustomerController(ICustomerService customer)
        {
            _customer=customer;
        }

        /// <summary>
        /// Get all customers.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the agency to retrieve a list of all registered customers.
        ///
        /// The response includes basic information about each customer, such as their name, email, and registration date.
        /// </remarks>
        /// <returns>Returns a list of all registered customers.</returns>
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(SuccessResponse<List<Customer>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCustomer()
        {
            var customers = _customer.GetAll();
            return Ok(new SuccessResponse<List<Customer>>(customers.ToList()));
        }
    }
}
