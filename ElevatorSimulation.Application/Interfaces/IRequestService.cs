using ElevatorSimulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Interfaces
{
    /// <summary>
    /// Defines the operations for handling requests to dispatch an elevator.
    /// </summary>
    public interface IRequestService
    {
        /// <summary>
        /// Dispatches an elevator to fulfill the specified request.
        /// </summary>
        /// <param name="elevators">The list of available elevators to choose from.</param>
        /// <param name="request">The request that specifies which elevator should be dispatched.</param>
        void DispatchElevator(List<Elevator> elevators, Request request);
    }

}
