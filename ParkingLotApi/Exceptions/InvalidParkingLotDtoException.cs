using System.Net;

namespace ParkingLotApi.Exceptions
{

    public class InvalidParkingLotDtoException : HttpResponseException
    {
        public InvalidParkingLotDtoException(string errorMessage): base(errorMessage)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
