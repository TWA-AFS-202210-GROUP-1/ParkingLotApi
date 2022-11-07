using System.Net;

namespace ParkingLotApi.Exceptions
{
  public class DuplicateParkingLotNameException : ParkingLotApiExceptionBase
  {
    public DuplicateParkingLotNameException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
  }
}
