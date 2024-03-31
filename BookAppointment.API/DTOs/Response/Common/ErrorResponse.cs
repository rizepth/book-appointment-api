namespace BookAppointment.API.DTOs.Response.Common
{
    public class ErrorResponse : BaseResponse
    {
        public List<string> Errors { get; set; }

        public ErrorResponse(List<string> errors)
        {
            Success = false;
            Errors = errors;
        }

        public ErrorResponse(string error)
        {
            Success = false;
            Errors = new List<string> { error };
        }
    }
}
