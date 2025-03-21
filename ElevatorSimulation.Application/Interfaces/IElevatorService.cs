using ElevatorSimulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Interfaces
{
    /// <summary>
    /// Defines the operations available for controlling an elevator.
    /// </summary>
    public interface IElevatorService
    {
        /// <summary>
        /// Moves the elevator to the specified target floor.
        /// </summary>
        /// <param name="elevator">The elevator to be moved.</param>
        /// <param name="targetFloor">The target floor to move the elevator to.</param>
        void MoveElevator(Elevator elevator, int targetFloor);

        /// <summary>
        /// Boards passengers into the elevator.
        /// Returns a tuple: (number boarded, number left waiting).
        /// </summary>
        (int boarded, int left) BoardPassengers(Elevator elevator, int passengers);

        /// <summary>
        /// Processes the next request in the queue for the specified elevator.
        /// </summary>
        /// <param name="elevator">The elevator to process the next request for.</param>
        void ProcessNextRequest(Elevator elevator);
    }

}
