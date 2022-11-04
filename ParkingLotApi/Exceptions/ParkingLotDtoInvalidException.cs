using System.Net;

namespace ParkingLotApi.Exceptions
{

    public class ParkingLotDtoInvalidException : HttpResponseException
    {
        public ParkingLotDtoInvalidException(string errorMessage)
        {
            StatusCode = HttpStatusCode.BadRequest;
            ErrorMessage = errorMessage;
        }
    }
}
