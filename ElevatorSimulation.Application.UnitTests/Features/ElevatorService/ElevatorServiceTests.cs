using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Application.Logging;
using ElevatorSimulation.Application.Services;
using ElevatorSimulation.Domain.Entities;
using ElevatorSimulation.Domain.Enums;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.UnitTests.Features.ElevatorService
{
    public class ElevatorServiceTests
    {
        [Fact]
        public void MoveElevator_Should_Update_CurrentFloor_And_Log_Message()
        {
            // Arrange
            var mockDispatchingStrategy = new Mock<IDispatchingStrategy>();
            var requestQueue = new RequestQueue();
            var mockLogger = new Mock<IAppLogger<Services.ElevatorService>>();
            var elevatorService = new Services.ElevatorService(mockDispatchingStrategy.Object, requestQueue, mockLogger.Object);
            var elevator = new Elevator(1, 5, 10, ElevatorType.Passenger);

            // Act
            elevatorService.MoveElevator(elevator, 3);

            // Assert
            elevator.GetCurrentFloor().ShouldBe(3);
            mockLogger.Verify(
                logger => logger.LogInformation("Elevator {0} moved to floor {1}", elevator.Id, 3),
                Times.Once);
        }

        [Fact]
        public void BoardPassengers_Should_Board_Correct_Number_And_Log()
        {
            // Arrange
            var mockDispatchingStrategy = new Mock<IDispatchingStrategy>();
            var requestQueue = new RequestQueue();
            var mockLogger = new Mock<IAppLogger<Services.ElevatorService>>();
            var elevatorService = new Services.ElevatorService(mockDispatchingStrategy.Object, requestQueue, mockLogger.Object);
            var elevator = new Elevator(1, 5, 10, ElevatorType.Passenger);

            // Act: Request to board 8 passengers.
            var result = elevatorService.BoardPassengers(elevator, 8);

            // Assert: Elevator should board 8 and leave 0 waiting.
            result.boarded.ShouldBe(8);
            result.left.ShouldBe(0);
            mockLogger.Verify(
                logger => logger.LogInformation("Elevator {0} boarded {1} passengers.", elevator.Id, 8),
                Times.Once);

            // Act: Now try boarding 5 more (only 2 available since capacity is 10).
            var result2 = elevatorService.BoardPassengers(elevator, 5);

            // Assert: Elevator should board 2 and leave 3 waiting.
            result2.boarded.ShouldBe(2);
            result2.left.ShouldBe(3);
            mockLogger.Verify(
                logger => logger.LogWarning("Elevator {0} could not board {1} passengers due to capacity limits.", elevator.Id, 3),
                Times.Once);
        }

        [Fact]
        public void ProcessNextRequest_Should_Process_Queued_Request()
        {
            // Arrange
            var mockDispatchingStrategy = new Mock<IDispatchingStrategy>();
            var requestQueue = new RequestQueue();
            var mockLogger = new Mock<IAppLogger<Services.ElevatorService>>();
            var elevatorService = new Services.ElevatorService(mockDispatchingStrategy.Object, requestQueue, mockLogger.Object);
            var elevator = new Elevator(1, 6, 10, ElevatorType.Passenger);

            // Enqueue a request: elevator should move from floor 6 to floor 4 and board 2 passengers.
            requestQueue.EnqueueRequest(new Request(4, 2));

            // Act: Process the next request.
            elevatorService.ProcessNextRequest(elevator);

            // Assert:
            // After processing, elevator's current floor should be 4 and it should have boarded 2 passengers.
            elevator.GetCurrentFloor().ShouldBe(4);
            elevator.GetOccupants().ShouldBe(2);

            // Verify that direction was set and MoveElevator and BoardPassengers were called.
            mockLogger.Verify(
                logger => logger.LogInformation("Elevator {0} direction set to {1}", elevator.Id, It.IsAny<Direction>()),
                Times.Once);
            mockLogger.Verify(
                logger => logger.LogInformation("Elevator {0} moved to floor {1}", elevator.Id, 4),
                Times.Once);
            mockLogger.Verify(
                logger => logger.LogInformation("Elevator {0} boarded {1} passengers.", elevator.Id, 2),
                Times.Once);
        }
    }
}
