using System.Net;

namespace ParkingLotApi.Exceptions
{
  public class ParkingLotPageIndexOutOfRangeException : ParkingLotApiExceptionBase
  {
    public ParkingLotPageIndexOutOfRangeException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
  }
}
