using System;
using System.Net;

namespace ParkingLotApi.Exceptions;

public class NotFoundEntityException : HttpResponseException
{
    public NotFoundEntityException(string errorMessage): base(errorMessage)
    {
        StatusCode = HttpStatusCode.NotFound;
    }
}