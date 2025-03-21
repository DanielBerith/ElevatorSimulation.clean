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
    /// <summary>
    /// Dispatches an elevator based on the requested floor and assigns a specific elevator type.
    /// Example rules:
    ///   - Floor 1 → Passenger elevator.
    ///   - Floor 3 → Freight elevator.
    ///   - Floor 6 → HighSpeed elevator.
    /// </summary>
    public class FloorBasedDispatchingStrategy : IDispatchingStrategy
    {
        public Elevator DispatchElevator(List<Elevator> elevators, int requestedFloor)
        {
            ElevatorType requiredType;
            if (requestedFloor == 3)
                requiredType = ElevatorType.Freight;
            else if (requestedFloor == 1)
                requiredType = ElevatorType.Passenger;
            else if (requestedFloor == 6)
                requiredType = ElevatorType.HighSpeed;
            else
                requiredType = ElevatorType.Passenger;

            var filteredElevators = elevators.Where(e => e.Type == requiredType).ToList();
            if (!filteredElevators.Any())
                return null;

            return filteredElevators.OrderBy(e => Math.Abs(e.GetCurrentFloor() - requestedFloor)).FirstOrDefault();
        }
    }
}
