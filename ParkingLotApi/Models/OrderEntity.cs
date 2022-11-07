using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingLotApi.Models
{
    public class Order
    {
        public Order()
        {
        }

        [Key]
        public int OrderNumber { get; set; }
        public string NameOfParkingLot { get; set; }
        public string CarPlateNumber { get; set; }
        public string CreateTime { get; set; }
        public string CloseTime { get; set; }
        public string OrderStatus { get; set; }
    }
}