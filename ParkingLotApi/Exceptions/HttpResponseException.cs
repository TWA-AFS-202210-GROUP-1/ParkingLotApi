using System;
using System.Net;

namespace ParkingLotApi.Exceptions;

public class HttpResponseException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public string ErrorMessage { get; set; }
}