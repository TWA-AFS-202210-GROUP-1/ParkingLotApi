using ParkingLotApi.Consts;
using System;
using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos;

public class CreateParkingOrderDto
{
    public int ParkingLotId { get; set; }
    public string PlateNumber { get; set; }

    public CreateParkingOrderDto(int parkingLotId, string plateNumber)
    {
        ParkingLotId = parkingLotId;
        PlateNumber = plateNumber;
    }

    public ParkingOrderEntity ToEntity()
    {
        return new ParkingOrderEntity()
        {
            CreateTime = DateTime.Now,
            PlateNumber = PlateNumber,
            Status = OrderStatus.Open,
        };
    }
}