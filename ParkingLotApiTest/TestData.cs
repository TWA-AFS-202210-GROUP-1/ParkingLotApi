using ParkingLotApiTest.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotApiTest
{
    public class TestData
    {
        public static List<ParkingLotDto> ParkingLotDtos = new List<ParkingLotDto>()
        {
            new ParkingLotDto
            {
                Name = "park1",
                Capacity = 10,
                Location = "Chaoyang1",
            },
            new ParkingLotDto
            {
                Name = "park2",
                Capacity = 100,
                Location = "Chaoyang2",
            },
            new ParkingLotDto
            {
                Name = "park3",
                Capacity = 20,
                Location = "Chaoyang3",
            },
            new ParkingLotDto
            {
                Name = "park4",
                Capacity = 60,
                Location = "Chaoyang4",
            }
        };
    }
}
