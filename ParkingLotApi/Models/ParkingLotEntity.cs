using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParkingLotApi.Models
{
    public class ParkingLot
    {
        public ParkingLot()
        {
        }

        [Key]
        public int ParkingLotId { get; set; }

        public string ParkingLotName { get; set; }

        public int ParkingLotCapacity { get; set; }

        public string ParkingLotLocation { get; set; }

        public List<Order>? OrdersListEntity { get; set; }
    }
}
