using System.Net;

namespace ParkingLotApi.Exceptions
{
  public class ParkingLotFullException : ParkingLotApiExceptionBase
  {
    public ParkingLotFullException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
  }
}
