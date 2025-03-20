using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IElevatorService _elevatorService;
        private readonly IDispatchingStrategy _dispatchingStrategy;
        public RequestService(IElevatorService elevatorService, IDispatchingStrategy dispatchingStrategy)
        {
            _elevatorService = elevatorService;
            _dispatchingStrategy = dispatchingStrategy;
        }

        public void DispatchElevator(List<Elevator> elevators, Request request)
        {
            // Use the dispatching strategy to select the elevator
            var selectedElevator = _dispatchingStrategy.DispatchElevator(elevators, request.Floor);

            if (selectedElevator != null)
            {
                // Move the selected elevator to the requested floor
                _elevatorService.MoveElevator(selectedElevator, request.Floor);

                // Attempt to board passengers
                if (_elevatorService.TryBoardPassengers(selectedElevator, request.Passengers))
                {
                    Console.WriteLine($"Elevator {selectedElevator.Id} boarded {request.Passengers} passengers.");
                }
                else
                {
                    Console.WriteLine($"Elevator {selectedElevator.Id} cannot board {request.Passengers} passengers. It's full!");
                }
            }
            else
            {
                Console.WriteLine("No available elevator found!");
            }
        }
    }

}
