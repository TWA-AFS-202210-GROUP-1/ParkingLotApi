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
    public class ServiceTestBase : TestBase
    {
        public ServiceTestBase(WebApplicationFactory<Program> factory)
            : base(factory)
        {
            this.Factory = factory;
        }

        protected WebApplicationFactory<Program> Factory { get; }

        protected ParkingLotContext GetParkingLotContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopedService = scope.ServiceProvider;
            var context = scopedService.GetRequiredService<ParkingLotContext>();
            return context;
        }
    }
}