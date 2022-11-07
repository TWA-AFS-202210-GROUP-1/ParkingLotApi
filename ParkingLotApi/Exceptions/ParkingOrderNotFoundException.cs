using System.Net;

namespace ParkingLotApi.Exceptions
{
  public class ParkingOrderNotFoundException : ParkingLotApiExceptionBase
  {
    public ParkingOrderNotFoundException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
  }
}
