using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using ParkingLotApi.Repository;
    using ParkingLotApi.Service;
    using ParkingLotApiTest.Dtos;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;

    public class ParkingLotServiceTest : TestBase
    {
        public ParkingLotServiceTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parkingLot_success_via_parkingLot_service()
        {
            // given
            var context = GetParkingLotDbContext();
            IParkingLotService parkingLotService = new ParkingLotService(context);
            ParkingLotDto parkingLotDto = PrepareAddParkingLotDto();
            // when
            await parkingLotService.AddParkingLot(parkingLotDto);

            // then
            Assert.Equal(1, context.ParkingLots.Count());
        }

        //[Fact]
        //public async Task Should_get_company_byId_success_via_company_service()
        //{
        //    // given
        //    var context = GetCompanyDbContext();
        //    CompanyService companyService = new CompanyService(context);
        //    CompanyDto companyDto = PrepareAddCompanyDto();
        //    var targetId = await companyService.AddCompany(companyDto);
        //    // when
        //    CompanyDto targetCompany = await companyService.GetById(targetId);

        //    // then
        //    Assert.Equal("IBM", targetCompany.Name);
        //}

        //[Fact]
        //public async Task Should_get_all_companies_success_via_company_service()
        //{
        //    // given
        //    var context = GetCompanyDbContext();
        //    CompanyService companyService = new CompanyService(context);
        //    PrepareCompaniesDto(companyService);
        //    //when
        //    List<CompanyDto> targetCompanies = await companyService.GetAll();
        //    //then
        //    Assert.Equal("slb", targetCompanies[1].Name);
        //    Assert.Equal(1, targetCompanies[1].EmployeesDto.Count);
        //}

        //[Fact]
        //public async Task Should_delete_a_company_by_id_success_via_company_service()
        //{
        //    // given
        //    var context = GetCompanyDbContext();
        //    CompanyService companyService = new CompanyService(context);
        //    var companiesIds = PrepareCompaniesDto(companyService);
        //    //when
        //    await companyService.DeleteCompany(await companiesIds[0]);
        //    //then
        //    Assert.Equal(1, context.Companies.Count());
        //}

        private ParkingLotDto PrepareAddParkingLotDto()
        {
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "park1",
                Capacity = 10,
                Location = "Chaoyang",
            };
            return parkingLotDto;
        }
        private ParkingLotContext GetParkingLotDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopedService = scope.ServiceProvider;
            ParkingLotContext context = scopedService.GetRequiredService<ParkingLotContext>();
            return context;

        }
    }
}