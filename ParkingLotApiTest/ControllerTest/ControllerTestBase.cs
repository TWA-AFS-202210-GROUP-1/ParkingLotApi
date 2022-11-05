using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ParkingLotApi.Repository;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkingLotApiTest.ControllerTest;

public class ControllerTestBase : IDisposable
{
    protected readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _factory;
    public ControllerTestBase()
    {
        _factory = new WebApplicationFactory<Program>();
        _httpClient = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ParkingLotContext>));
                services.Remove(descriptor);

                services.AddDbContext<ParkingLotContext>(options =>
                {
                    InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(options, "InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ParkingLotContext>();
                    db.Database.EnsureCreated();
                }
            });
        }).CreateClient();
    }
    public void Dispose()
    {
        var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<ParkingLotContext>();

        context.ParkingOrders.RemoveRange(context.ParkingOrders);
        context.ParkingLots.RemoveRange(context.ParkingLots);

        context.SaveChanges();
    }

    protected async Task<T> GetObjectFromHttpResponse<T>(HttpResponseMessage httpResponse) where T : class
    {
        var jsonString = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}