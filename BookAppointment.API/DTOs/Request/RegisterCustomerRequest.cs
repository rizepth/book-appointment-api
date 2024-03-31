namespace BookAppointment.API.DTOs.Request
{
    public class RegisterCustomerRequest : RegisterRequest
    {
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
