using ElevatorSimulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Domain.Entities
{
    public class HighSpeedElevator : Elevator
    {
        public HighSpeedElevator(int id, int initialFloor, int maxCapacity)
            : base(id, initialFloor, maxCapacity, ElevatorType.HighSpeed) { }

        // Override the MoveToFloor method to simulate faster movement
        public override void MoveToFloor(int targetFloor)
        {
            Console.WriteLine($"High-Speed Elevator {Id} is moving quickly to floor {targetFloor}.");
            base.MoveToFloor(targetFloor); // Calls the base MoveToFloor method to simulate movement
        }
    }
}
