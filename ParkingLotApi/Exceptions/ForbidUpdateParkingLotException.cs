using System;
using System.Net;

namespace ParkingLotApi.Exceptions;

public class ForbidUpdateParkingLotException : HttpResponseException
{
    public ForbidUpdateParkingLotException(string errorMessage) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}