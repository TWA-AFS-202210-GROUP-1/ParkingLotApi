using ParkingLotApi.Models;
using System.Xml.Linq;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public ParkingLotDto(ParkingLotEntity parkingLotEntity)
        {
            this.ParkingLotName = parkingLotEntity.ParkingLotName;
            this.ParkingLotCapacity = parkingLotEntity.ParkingLotCapacity;
            this.ParkingLotLocation = parkingLotEntity.ParkingLotLocation;
        }

        public ParkingLotDto()
        {
        }

        public string ParkingLotName { get; set; }

        public int ParkingLotCapacity { get; set; }

        public string ParkingLotLocation { get; set; }

        public ParkingLotEntity ToParkingLotEntity()
        {
            return new ParkingLotEntity()
            {
                ParkingLotName = this.ParkingLotName,
                ParkingLotCapacity = this.ParkingLotCapacity,
                ParkingLotLocation = this.ParkingLotLocation,
            };
        }
    }
}