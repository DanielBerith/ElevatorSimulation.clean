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

        public void MoveElevator(Elevator elevator, int targetFloor)
        {
            elevator.MoveToFloor(targetFloor);
            _logger.LogInformation("Elevator {0} moved to floor {1}", elevator.Id, targetFloor);
        }

        public bool TryBoardPassengers(Elevator elevator, int passengers)
        {
            bool success = elevator.TryBoardPassengers(passengers);
            if (success)
            {
                _logger.LogInformation("Elevator {0} boarded {1} passengers.", elevator.Id, passengers);
            }
            else
            {
                _logger.LogWarning("Elevator {0} cannot board {1} passengers. It's full!", elevator.Id, passengers);
            }
            return success;
        }

        // Process requests by dispatching the nearest request in the queue based on current direction
        public void ProcessNextRequest(Elevator elevator)
        {
            // Set initial direction based on the first request in the queue
            Direction currentDirection = Direction.Idle;

            // Get the first request to decide the initial direction
            var firstRequest = _requestQueue.DequeueRequest(elevator.GetCurrentFloor(), currentDirection);

            if (firstRequest != null)
            {
                currentDirection = firstRequest.Floor > elevator.GetCurrentFloor() ? Direction.Up : Direction.Down;
                _logger.LogInformation("Elevator {0} direction set to {1}", elevator.Id, currentDirection);

                // Process requests in the current direction
                while (firstRequest != null)
                {
                    // Get the nearest request based on the current direction
                    var nextRequest = _requestQueue.DequeueRequest(elevator.GetCurrentFloor(), currentDirection);
                    if (nextRequest != null)
                    {
                        // Move the elevator to the requested floor and log the action
                        MoveElevator(elevator, nextRequest.Floor);

                        // Attempt to board passengers and log the outcome
                        if (TryBoardPassengers(elevator, nextRequest.Passengers))
                        {
                            _logger.LogInformation("Elevator {0} successfully boarded {1} passengers.", elevator.Id, nextRequest.Passengers);
                        }
                        else
                        {
                            _logger.LogWarning("Elevator {0} failed to board {1} passengers due to capacity constraints.", elevator.Id, nextRequest.Passengers);
                        }
                    }
                    else
                    {
                        // No more requests in the current direction
                        break;
                    }
                }
            }
            else
            {
                _logger.LogInformation("No request in the queue to process for Elevator {0}.", elevator.Id);
            }
        }
    }
}
