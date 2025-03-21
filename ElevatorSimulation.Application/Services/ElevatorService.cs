using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Application.Logging;
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
    /// Provides operations to move an elevator and board passengers.
    /// </summary>
    public class ElevatorService : IElevatorService
    {
        private readonly IDispatchingStrategy _dispatchingStrategy;
        private readonly RequestQueue _requestQueue;
        private readonly IAppLogger<ElevatorService> _logger;

        public ElevatorService(IDispatchingStrategy dispatchingStrategy, RequestQueue requestQueue, IAppLogger<ElevatorService> logger)
        {
            _dispatchingStrategy = dispatchingStrategy;
            _requestQueue = requestQueue;
            _logger = logger;
        }

        /// <summary>
        /// Moves the given elevator to the target floor and logs the action.
        /// </summary>
        public void MoveElevator(Elevator elevator, int targetFloor)
        {
            elevator.MoveToFloor(targetFloor);
            _logger.LogInformation("Elevator {0} moved to floor {1}", elevator.Id, targetFloor);
        }

        /// <summary>
        /// Attempts to board passengers into the given elevator and logs the outcome.
        /// </summary>
        public (int boarded, int left) BoardPassengers(Elevator elevator, int passengers)
        {
            var result = elevator.BoardPassengers(passengers);
            if (result.boarded > 0)
            {
                _logger.LogInformation("Elevator {0} boarded {1} passengers.", elevator.Id, result.boarded);
            }
            if (result.left > 0)
            {
                _logger.LogWarning("Elevator {0} could not board {1} passengers due to capacity limits.", elevator.Id, result.left);
            }
            return result;
        }

        /// <summary>
        /// Processes the next request from the request queue for the specified elevator.
        /// (This method is useful when an elevator is continuously processing queued requests.)
        /// </summary>
        public void ProcessNextRequest(Elevator elevator)
        {
            // Determine initial direction from the first request; if idle, select the nearest.
            Direction currentDirection = Direction.Idle;
            var firstRequest = _requestQueue.DequeueRequest(elevator.GetCurrentFloor(), currentDirection);
            if (firstRequest != null)
            {
                currentDirection = firstRequest.Floor > elevator.GetCurrentFloor() ? Direction.Up : Direction.Down;
                _logger.LogInformation("Elevator {0} direction set to {1}", elevator.Id, currentDirection);

                Request nextRequest = firstRequest;
                while (nextRequest != null)
                {
                    MoveElevator(elevator, nextRequest.Floor);
                    var boardingResult = BoardPassengers(elevator, nextRequest.Passengers);
                    if (boardingResult.left > 0)
                    {
                        _logger.LogInformation("Elevator {0} boarded {1} passengers; {2} waiting.", elevator.Id, boardingResult.boarded, boardingResult.left);
                    }
                    nextRequest = _requestQueue.DequeueRequest(elevator.GetCurrentFloor(), currentDirection);
                }
            }
            else
            {
                _logger.LogInformation("No request in the queue to process for Elevator {0}.", elevator.Id);
            }
        }
    }
}
