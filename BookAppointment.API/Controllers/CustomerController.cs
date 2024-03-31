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

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCustomer()
        {
            var customers = _customer.GetAll();
            return Ok(new SuccessResponse<List<Customer>>(customers.ToList()));
        }
    }
}
