using System;
using System.Net;

namespace ParkingLotApi.Exceptions;

public class FullParkingLotException : HttpResponseException
{
    public FullParkingLotException(string errorMessage) : base(errorMessage)
    {
        StatusCode = HttpStatusCode.Conflict;
    }
}