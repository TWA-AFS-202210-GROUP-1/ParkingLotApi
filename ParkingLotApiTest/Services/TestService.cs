using Newtonsoft.Json;
using ParkingLotApi.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParkingLotApiTest.Services
{
  public static class TestService
  {
    public static StringContent SerializeDto<T>(T requestDto)
    {
      var serializedDto = JsonConvert.SerializeObject(requestDto);
      var requestBody = new StringContent(serializedDto, Encoding.UTF8, MediaTypeNames.Application.Json);

      return requestBody;
    }

    public static List<StringContent> SerializeDtoList<T>(List<T> requestDtoList)
    {
      var serializedDtos = requestDtoList
        .Select(requestDto => JsonConvert.SerializeObject(requestDto));
      var requestBodyList = serializedDtos
        .Select(serializedDto => new StringContent(serializedDto, Encoding.UTF8, MediaTypeNames.Application.Json))
        .ToList();

      return requestBodyList;
    }

    public static async Task<T?> GetResponseContents<T>(HttpResponseMessage response)
    {
      if (response.Content != null)
      {
        var responseBody = await response.Content.ReadAsStringAsync();
        var deserializedObject = JsonConvert.DeserializeObject<T>(responseBody);

        return deserializedObject;
      }
      else
      {
        return default;
      }
    }

    public static async Task<List<int>> PostDtoList<T>(HttpClient httpClient, string url, List<T> requestDtoList)
    {
      var idList = new List<int>();
      var requestBodyList = SerializeDtoList(requestDtoList);
      foreach (var requestBody in requestBodyList)
      {
        var response = await httpClient.PostAsync(url, requestBody);
        var idString = await response.Content.ReadAsStringAsync();
        idList.Add(int.Parse(idString));
      }

      return idList;
    }

    public static List<ParkingLotDto> PrepareParkingLotDtos()
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
