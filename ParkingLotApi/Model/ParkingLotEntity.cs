using System;
using System.Collections.Generic;

namespace ParkingLotApi.Model
{
    public class ParkingLotEntity
    {
        public ParkingLotEntity()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public List<ParkingOrderEntity> ParkingOrders { get; set; } = new List<ParkingOrderEntity>();

    }
}
