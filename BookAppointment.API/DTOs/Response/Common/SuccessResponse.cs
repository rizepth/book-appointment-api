namespace BookAppointment.API.DTOs.Response.Common
{
    public class SuccessResponse<T> : BaseResponse
    {
        public T Data { get; set; }

        public SuccessResponse(T data = default, string message = "success")
        {
            Success = true;
            Data = data;
            Message = message;
        }
    }
}
