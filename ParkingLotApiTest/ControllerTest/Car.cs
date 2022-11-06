using ParkingLotApi.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ParkingLotApiTest.ControllerTest
{
    public class Car
    {
        private string carPlateNumber;

        public Car(string carPlateNumber)
        {
            this.carPlateNumber = carPlateNumber;
        }

        public async Task<HttpResponseMessage> Parking(HttpClient client, HttpResponseMessage createParkingLotResponse)
        {
            var createOrderResponseBody = await createParkingLotResponse.Content.ReadAsStringAsync();
            var returnParkingLotDto = JsonConvert.DeserializeObject<ParkingLotDto>(createOrderResponseBody);

            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                ParkingLotName = returnParkingLotDto.ParkingLotName,
                ParkingLotCapacity = returnParkingLotDto.ParkingLotCapacity,
                ParkingLotLocation = returnParkingLotDto.ParkingLotLocation,
                OrdersList = new List<OrderDto>()
                {
                    new OrderDto()
                    {
                        NameOfParkingLot = returnParkingLotDto.ParkingLotName,
                        CarPlateNumber = this.carPlateNumber,
                        CreateTime = DateTime.Now.ToString(),
                        CloseTime = string.Empty,
                        OrderStatus = "Open",
                    },
                },
            };
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            return await client.PatchAsync(createParkingLotResponse.Headers.Location, content);
        }
    }
}