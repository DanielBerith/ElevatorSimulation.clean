using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Services
{
    public class NearestElevatorStrategy : IDispatchingStrategy
    {
        public Elevator DispatchElevator(List<Elevator> elevators, int requestedFloor)
        {
            // This strategy selects the nearest elevator based on the requested floor
            return elevators.OrderBy(e => Math.Abs(e.GetCurrentFloor() - requestedFloor)).FirstOrDefault();
        }
    }
}
