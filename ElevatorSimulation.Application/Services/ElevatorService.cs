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
        public void ProcessNextRequest(List<Elevator> elevators)
        {
            // Get the next request from the queue
            var nextRequest = _requestQueue.DequeueNearestRequest(elevators.First().GetCurrentFloor());

            if (nextRequest != null)
            {
                // Use the dispatch strategy to get the closest elevator
                var elevator = _dispatchingStrategy.DispatchElevator(elevators, nextRequest.Floor);

                if (elevator != null)
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
                    Console.WriteLine("No available elevator found.");
                }
            }
            else
            {
                Console.WriteLine("No requests left in the queue.");
            }
        }
    }
}
