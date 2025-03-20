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
        /// Attempts to board passengers into the elevator.
        /// </summary>
        /// <param name="elevator">The elevator where passengers will board.</param>
        /// <param name="passengers">The number of passengers attempting to board.</param>
        /// <returns>True if passengers were successfully boarded, otherwise false.</returns>
        bool TryBoardPassengers(Elevator elevator, int passengers);
    }

}
