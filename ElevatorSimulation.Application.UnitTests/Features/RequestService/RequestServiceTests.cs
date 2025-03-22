using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Application.Logging;
using ElevatorSimulation.Domain.Entities;
using ElevatorSimulation.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.UnitTests.Features.RequestService
{
    public class RequestServiceTests
    {
        [Fact]
        public void DispatchElevator_Should_Dispatch_When_Elevator_Is_Found()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1, 5, 10, ElevatorType.Passenger),
                new Elevator(2, 3, 10, ElevatorType.Freight)
            };

            // Create a request (e.g., request floor 3 with 4 passengers)
            var request = new Request(3, 4);

            // Create a mock for IElevatorService.
            var mockElevatorService = new Mock<IElevatorService>();
            // Setup BoardPassengers to return a tuple (all passengers boarded, none left).
            mockElevatorService.Setup(es => es.BoardPassengers(It.IsAny<Elevator>(), It.IsAny<int>()))
                .Returns((Elevator e, int p) => (p, 0));
            // Setup MoveElevator to do nothing (we only verify it's called).
            mockElevatorService.Setup(es => es.MoveElevator(It.IsAny<Elevator>(), It.IsAny<int>()));

            // Create a mock for IDispatchingStrategy.
            var mockDispatchingStrategy = new Mock<IDispatchingStrategy>();
            // Return the second elevator for this request.
            mockDispatchingStrategy
                .Setup(ds => ds.DispatchElevator(elevators, request.Floor))
                .Returns(elevators[1]);

            // Create a mock for IAppLogger<RequestService>.
            var mockLogger = new Mock<IAppLogger<Services.RequestService>>();

            // Create an instance of RequestService with the mocks.
            var requestService = new Services.RequestService(mockElevatorService.Object, mockDispatchingStrategy.Object, mockLogger.Object);

            // Act: Dispatch the elevator.
            requestService.DispatchElevator(elevators, request);

            // Assert
            // Verify that the dispatching strategy was called with the correct parameters.
            mockDispatchingStrategy.Verify(ds => ds.DispatchElevator(elevators, request.Floor), Times.Once);
            // Verify that MoveElevator was called with the selected elevator and requested floor.
            mockElevatorService.Verify(es => es.MoveElevator(elevators[1], request.Floor), Times.Once);
            // Verify that BoardPassengers was called with the correct parameters.
            mockElevatorService.Verify(es => es.BoardPassengers(elevators[1], request.Passengers), Times.Once);
            // Verify logging calls.
            mockLogger.Verify(logger => logger.LogInformation("Dispatching elevator for request at floor {0} with {1} passengers", request.Floor, request.Passengers), Times.Once);
            mockLogger.Verify(logger => logger.LogInformation("Selected Elevator {0} (Type: {1}) for the request", elevators[1].Id, elevators[1].Type), Times.Once);
        }

        [Fact]
        public void DispatchElevator_Should_Log_Warning_When_No_Elevator_Found()
        {
            // Arrange
            var elevators = new List<Elevator>(); // No available elevators.
            var request = new Request(3, 4);

            var mockElevatorService = new Mock<IElevatorService>();

            var mockDispatchingStrategy = new Mock<IDispatchingStrategy>();
            // Simulate no available elevator.
            mockDispatchingStrategy.Setup(ds => ds.DispatchElevator(elevators, request.Floor))
                .Returns((Elevator)null);

            var mockLogger = new Mock<IAppLogger<Services.RequestService>>();

            var requestService = new Services.RequestService(mockElevatorService.Object, mockDispatchingStrategy.Object, mockLogger.Object);

            // Act: Dispatch the elevator.
            requestService.DispatchElevator(elevators, request);

            // Assert
            // Verify that the dispatching strategy was called.
            mockDispatchingStrategy.Verify(ds => ds.DispatchElevator(elevators, request.Floor), Times.Once);
            // Verify that MoveElevator and BoardPassengers were not called.
            mockElevatorService.Verify(es => es.MoveElevator(It.IsAny<Elevator>(), It.IsAny<int>()), Times.Never);
            mockElevatorService.Verify(es => es.BoardPassengers(It.IsAny<Elevator>(), It.IsAny<int>()), Times.Never);
            // Verify that a warning was logged.
            mockLogger.Verify(logger => logger.LogWarning("No available elevator found for request at floor {0}", request.Floor), Times.Once);
        }
    }
}
