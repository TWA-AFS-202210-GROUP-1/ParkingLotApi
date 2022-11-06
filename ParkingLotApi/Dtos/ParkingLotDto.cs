using ParkingLotApi.Model;

namespace ParkingLotApiTest.Dtos
{
    public class ParkingLotDto
    {
        public ParkingLotDto()
        {
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
                Name = this.Name,
                Capacity = this.Capacity,
                Location = this.Location,
            };
        }
    }
}