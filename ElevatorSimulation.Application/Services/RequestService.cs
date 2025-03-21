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
    /// <summary>
    /// Coordinates elevator dispatching based on incoming requests.
    /// </summary>
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

        /// <summary>
        /// Dispatches an elevator to service a request.
        /// The strategy selects an elevator based on the requested floor.
        /// </summary>
        public void DispatchElevator(List<Elevator> elevators, Request request)
        {
            _logger.LogInformation("Dispatching elevator for request at floor {0} with {1} passengers", request.Floor, request.Passengers);
            var selectedElevator = _dispatchingStrategy.DispatchElevator(elevators, request.Floor);
            if (selectedElevator != null)
            {
                _logger.LogInformation("Selected Elevator {0} (Type: {1}) for the request", selectedElevator.Id, selectedElevator.Type);
                _elevatorService.MoveElevator(selectedElevator, request.Floor);
                var boardingResult = _elevatorService.BoardPassengers(selectedElevator, request.Passengers);
                if (boardingResult.left > 0)
                {
                    _logger.LogInformation("Elevator {0} boarded {1} passengers; {2} remain waiting.", selectedElevator.Id, boardingResult.boarded, boardingResult.left);
                }
            }
            else
            {
                _logger.LogWarning("No available elevator found for request at floor {0}", request.Floor);
            }
        }
    }

}
