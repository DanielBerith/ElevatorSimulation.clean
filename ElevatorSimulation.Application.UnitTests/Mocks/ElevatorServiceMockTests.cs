using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Domain.Entities;
using ElevatorSimulation.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.UnitTests.Mocks
{
    public class ElevatorServiceMockTests
    {
        [Fact]
        public void ElevatorService_Methods_Should_Be_Called_As_Expected()
        {
            // Create a mock for IElevatorService
            var mockElevatorService = new Mock<IElevatorService>();

            // Setup MoveElevator method to do nothing (just verify it's called)
            mockElevatorService.Setup(es => es.MoveElevator(It.IsAny<Elevator>(), It.IsAny<int>()));

            // Setup BoardPassengers method to return a tuple where all requested passengers are boarded and none are left.
            mockElevatorService.Setup(es => es.BoardPassengers(It.IsAny<Elevator>(), It.IsAny<int>()))
                .Returns((Elevator e, int passengers) => (passengers, 0));

            // Setup ProcessNextRequest to do nothing (we only want to verify its invocation)
            mockElevatorService.Setup(es => es.ProcessNextRequest(It.IsAny<Elevator>()));

            // Create a sample elevator instance.
            var elevator = new Elevator(1, 5, 10, ElevatorType.Passenger);

            // Act: call the methods via the mock's Object.
            mockElevatorService.Object.MoveElevator(elevator, 3);
            var boardingResult = mockElevatorService.Object.BoardPassengers(elevator, 5);
            mockElevatorService.Object.ProcessNextRequest(elevator);

            // Assert: check that the BoardPassengers returns the expected tuple.
            Assert.Equal((5, 0), boardingResult);

            // Verify that the methods were called exactly once with the expected parameters.
            mockElevatorService.Verify(es => es.MoveElevator(It.IsAny<Elevator>(), 3), Times.Once);
            mockElevatorService.Verify(es => es.BoardPassengers(It.IsAny<Elevator>(), 5), Times.Once);
            mockElevatorService.Verify(es => es.ProcessNextRequest(It.IsAny<Elevator>()), Times.Once);
        }
    }
}
