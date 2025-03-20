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

        public RequestService(IElevatorService elevatorService)
        {
            _elevatorService = elevatorService;
        }

        public void DispatchElevator(List<Elevator> elevators, Request request)
        {
            var closestElevator = elevators
                .OrderBy(e => Math.Abs(e.GetCurrentFloor() - request.Floor))
                .FirstOrDefault();

            if (closestElevator != null)
            {
                _elevatorService.MoveElevator(closestElevator, request.Floor);
                if (_elevatorService.TryBoardPassengers(closestElevator, request.Passengers))
                {
                    Console.WriteLine($"Elevator {closestElevator.Id} boarded {request.Passengers} passengers.");
                }
                else
                {
                    Console.WriteLine($"Elevator {closestElevator.Id} cannot board {request.Passengers} passengers. It's full!");
                }
            }
            else
            {
                Console.WriteLine("No available elevator found!");
            }
        }
    }

}
