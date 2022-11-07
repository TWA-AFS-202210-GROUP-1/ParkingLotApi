using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Mvc.Testing;
using ParkingLotApi.Repository;
using ParkingLotApi.Dtos;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace ParkingLotApiTest
{
    public class ControllerTestBase : TestBase
    {
        public ControllerTestBase(WebApplicationFactory<Program> factory) 
            : base(factory)
        {
            this.Factory = factory;
        }

        protected WebApplicationFactory<Program> Factory { get; }

        protected HttpClient GetClient()
        {
            return Factory.CreateClient();
        }

        protected async Task<StringContent> ConvertDtoToStringContent<T>(T t)
        {
            var httpContent = JsonConvert.SerializeObject(t);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            return content;
        }

        protected async Task<HttpResponseMessage> PostAsyncParkingLotDto(HttpClient client, ParkingLotDto parkingLotDto)
        {
            var content = this.ConvertDtoToStringContent(parkingLotDto).Result;
            return await client.PostAsync("/parkinglots", content);
        }

        protected async Task PostAsyncParkingLotDtoList(HttpClient client, List<ParkingLotDto> parkingLotDtos)
        {
            foreach (var parkingLotDto in parkingLotDtos)
            {
                await this.PostAsyncParkingLotDto(client, parkingLotDto);
            }
        }

        protected static async Task<T> DeserializeHttpResponse<T>(HttpResponseMessage httpResponse)
        {
            var body = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(body);
            return response;
        }

    }
}