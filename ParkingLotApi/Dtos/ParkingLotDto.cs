using System;
using System.Collections.Generic;
using System.Linq;
using ParkingLotApi.Models;
using ParkingLotApi.Services;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public ParkingLotDto()
        {
        }

        public ParkingLotDto(string name, int capacity, string location)
        {
            Name = name;
            Capacity = capacity;
            Location = location;
        }

        public ParkingLotDto(ParkingLotEntity parkingLotEntity)
        {
            Name = parkingLotEntity.Name;
            Capacity = parkingLotEntity.Capacity;
            Location = parkingLotEntity.Location;
        }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; }

        public ParkingLotEntity ToEntity()
        {
            return new ParkingLotEntity()
            {
                Name = Name,
                Capacity = Capacity,
                Location = Location,
            };
        }
    }
}
