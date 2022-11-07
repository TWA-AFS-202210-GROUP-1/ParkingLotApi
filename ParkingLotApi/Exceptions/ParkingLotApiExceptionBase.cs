using System;
using System.Net;

namespace ParkingLotApi.Exceptions
{
  public class ParkingLotApiExceptionBase : Exception
  {
    public ParkingLotApiExceptionBase(string message, HttpStatusCode statusCode)
    {
      Message = message;
      StatusCode = statusCode;
    }

    public override string Message { get; }

    public HttpStatusCode StatusCode { get; }
  }
}
