﻿using Newtonsoft.Json;
using ParkingLotApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParkingLotApiTest.Service
{
  public class TestService
  {
    public static StringContent SerializeDto<T>(T requestDto)
    {
      var serializedDto = JsonConvert.SerializeObject(requestDto);
      var requestBody = new StringContent(serializedDto, Encoding.UTF8, MediaTypeNames.Application.Json);

      return requestBody;
    }

    public static List<ParkingLotDto> PrepareTestParkingLots()
    {
      return new List<ParkingLotDto>
      {
        new ParkingLotDto("Park Xpert", 10, "Aaron's Hill, Surrey"),
        new ParkingLotDto("Mountain View Parking", 10, "Clapton, Berkshire"),
        new ParkingLotDto("Drive On Park", 10, "Stocksbridge, Sheffield"),
        new ParkingLotDto("Parker Parking", 10, "Knighton, City of Leicester"),
        new ParkingLotDto("Parking Miles", 10, "Stockstreet, Essex"),
      };
    }
  }
}
