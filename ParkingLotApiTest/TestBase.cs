using System.Net.Http;
using System;
using ParkingLotApi.Repository;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace ParkingLotApiTest;

public class TestBase : IClassFixture<ParkingLotWebApplicationFactory<Program>>, IDisposable
{
    public TestBase(ParkingLotWebApplicationFactory<Program> factory)
    {
        this.Factory = factory;
    }

    protected ParkingLotWebApplicationFactory<Program> Factory { get; }

    public void Dispose()
    {
        var scope = Factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<ParkingLotContext>();

        context.ParkingLots.RemoveRange(context.ParkingLots);

        context.SaveChanges();
    }

    protected HttpClient GetClient()
    {
        return Factory.CreateClient();
    }
}