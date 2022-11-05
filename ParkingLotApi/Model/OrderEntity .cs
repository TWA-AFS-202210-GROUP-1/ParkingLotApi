using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Model
{
    public class OrderEntity
    {
        public OrderEntity() { }

        public int Id { get; set; }

        public int Ordernumber { get; set; }

        public int NameofParkinglot { get; set; }

        public string PlateNumber { get; set; }

        public string CreationTime { get; set; }

        public string CloseTime { get; set; }

        public bool Status { get; set; }
    }
}
