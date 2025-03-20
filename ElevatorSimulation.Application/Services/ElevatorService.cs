using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Domain.Entities;
using ElevatorSimulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Services
{
    public class ElevatorService : IElevatorService
    {
        public void MoveElevator(Elevator elevator, int targetFloor)
        {
            elevator.MoveToFloor(targetFloor);
        }

        public bool TryBoardPassengers(Elevator elevator, int passengers)
        {
            return elevator.TryBoardPassengers(passengers);
        }
    }
}
