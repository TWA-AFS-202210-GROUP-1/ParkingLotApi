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
    public class TestBase : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        public TestBase(WebApplicationFactory<Program> factory)
        {
            this.Factory = factory;
        }

        protected WebApplicationFactory<Program> Factory { get; }

        public void Dispose()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<ParkingLotContext>();

            context.ParkingLots.RemoveRange(context.ParkingLots);
            context.SaveChanges();
        }

        protected List<ParkingLotDto> ParkingLotDtos()
        {
            var parkingLotDtos = new List<ParkingLotDto>()
            {
                new ParkingLotDto("PL1", 10, "Beijing"),
                new ParkingLotDto("PL2", 20, "Shanghai"),
                new ParkingLotDto("PL3", 30, "Wuhan"),
                new ParkingLotDto("PL4", 40, "Tianjin"),
            };
            return parkingLotDtos;
        }
    }
}