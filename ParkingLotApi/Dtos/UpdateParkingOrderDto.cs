using System;
using ParkingLotApi.Consts;
using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos;

public class UpdateParkingOrderDto
{
    public OrderStatus Status { get; set; }
    public UpdateParkingOrderDto(OrderStatus status)
    {
        Status = status;
    }

    public void UpdateEntity(ParkingOrderEntity entity)
    {
        entity.Status = Status;
    }
}