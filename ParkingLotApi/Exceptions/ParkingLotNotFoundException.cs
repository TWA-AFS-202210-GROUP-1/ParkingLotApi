using System.Net;

namespace ParkingLotApi.Exceptions
{
  public class ParkingLotNotFoundException : ParkingLotApiExceptionBase
  {
    public ParkingLotNotFoundException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
  }
}
