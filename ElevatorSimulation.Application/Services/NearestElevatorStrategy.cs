using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Services
{
    /// <summary>
    /// A simple dispatching strategy that selects the nearest elevator regardless of type.
    /// </summary>
    public class NearestElevatorStrategy : IDispatchingStrategy
    {
        public Elevator DispatchElevator(List<Elevator> elevators, int requestedFloor)
        {
            return elevators.OrderBy(e => Math.Abs(e.GetCurrentFloor() - requestedFloor)).FirstOrDefault();
        }
    }
}
