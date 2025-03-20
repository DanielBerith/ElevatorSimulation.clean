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
        private readonly IDispatchingStrategy _dispatchingStrategy;
        private readonly RequestQueue _requestQueue;

        // Constructor takes both the dispatch strategy and the request queue
        public ElevatorService(IDispatchingStrategy dispatchingStrategy, RequestQueue requestQueue)
        {
            _dispatchingStrategy = dispatchingStrategy;
            _requestQueue = requestQueue;
        }

        public void MoveElevator(Elevator elevator, int targetFloor)
        {
            elevator.MoveToFloor(targetFloor);
        }

        public bool TryBoardPassengers(Elevator elevator, int passengers)
        {
            return elevator.TryBoardPassengers(passengers);
        }

        // Process requests by dispatching the nearest elevator for the next request in the queue
        public void ProcessNextRequest(Elevator elevator)
        {
            // Set initial direction based on the first request in the queue
            Direction currentDirection = Direction.Idle;

            // Get the first request to decide the initial direction
            var firstRequest = _requestQueue.DequeueRequest(elevator.GetCurrentFloor(), currentDirection);

            if (firstRequest != null)
            {
                currentDirection = firstRequest.Floor > elevator.GetCurrentFloor() ? Direction.Up : Direction.Down;
                Console.WriteLine($"Elevator {elevator.Id} direction set to {currentDirection}");

                // Move elevator based on the direction
                while (firstRequest != null)
                {
                    // Get the nearest request based on the current direction
                    var nextRequest = _requestQueue.DequeueRequest(elevator.GetCurrentFloor(), currentDirection);
                    if (nextRequest != null)
                    {
                        // Move the elevator to the requested floor
                        MoveElevator(elevator, nextRequest.Floor);

                        // Try to board passengers
                        if (TryBoardPassengers(elevator, nextRequest.Passengers))
                        {
                            Console.WriteLine($"Elevator {elevator.Id} boarded {nextRequest.Passengers} passengers.");
                        }
                        else
                        {
                            Console.WriteLine($"Elevator {elevator.Id} cannot board {nextRequest.Passengers} passengers. It's full!");
                        }
                    }
                    else
                    {
                        // If no more requests in this direction, break out of the loop
                        break;
                    }
                }
            }
        }
    }
}
