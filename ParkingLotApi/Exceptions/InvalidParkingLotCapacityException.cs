using System.Net;

namespace ParkingLotApi.Exceptions
{
  public class InvalidParkingLotCapacityException : ParkingLotApiExceptionBase
  {
    public InvalidParkingLotCapacityException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
  }
}
