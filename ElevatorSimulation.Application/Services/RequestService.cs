using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Application.Logging;
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
        private readonly IAppLogger<RequestService> _logger;

        public RequestService(IElevatorService elevatorService, IDispatchingStrategy dispatchingStrategy, IAppLogger<RequestService> logger)
        {
            _elevatorService = elevatorService;
            _dispatchingStrategy = dispatchingStrategy;
            _logger = logger;
        }

        public void DispatchElevator(List<Elevator> elevators, Request request)
        {
            _logger.LogInformation("Dispatching elevator for request at floor {0} with {1} passengers", request.Floor, request.Passengers);

            // Use the dispatching strategy to select the elevator
            var selectedElevator = _dispatchingStrategy.DispatchElevator(elevators, request.Floor);

            if (selectedElevator != null)
            {
                _logger.LogInformation("Selected Elevator {0} for the request", selectedElevator.Id);

                // Move the selected elevator to the requested floor
                _elevatorService.MoveElevator(selectedElevator, request.Floor);

                // Attempt to board passengers
                if (_elevatorService.TryBoardPassengers(selectedElevator, request.Passengers))
                {
                    _logger.LogInformation("Elevator {0} successfully boarded {1} passengers", selectedElevator.Id, request.Passengers);
                }
                else
                {
                    _logger.LogWarning("Elevator {0} cannot board {1} passengers. It's full!", selectedElevator.Id, request.Passengers);
                }
            }
            else
            {
                _logger.LogWarning("No available elevator found for request at floor {0}", request.Floor);
            }
        }
    }

}
