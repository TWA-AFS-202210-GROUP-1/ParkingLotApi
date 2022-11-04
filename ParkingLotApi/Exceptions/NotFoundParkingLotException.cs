using System;
using System.Net;

namespace ParkingLotApi.Exceptions;

public class NotFoundParkingLotException : HttpResponseException
{
    public NotFoundParkingLotException(string errorMessage): base(errorMessage)
    {
        StatusCode = HttpStatusCode.NotFound;
    }
}