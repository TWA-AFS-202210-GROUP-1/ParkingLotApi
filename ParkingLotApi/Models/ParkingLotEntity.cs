using System.ComponentModel.DataAnnotations;

namespace ParkingLotApi.Models
{
    public class ParkingLotEntity
    {
        public ParkingLotEntity()
        {
        }

        [Key]
        public int ParkingLotId { get; set; }

        public string ParkingLotName { get; set; }

        public int ParkingLotCapacity { get; set; }

        public string ParkingLotLocation { get; set; }
    }
}
