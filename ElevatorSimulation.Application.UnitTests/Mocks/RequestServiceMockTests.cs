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
    public class RequestServiceMockTests
    {
        [Fact]
        public void DispatchElevator_Should_Be_Called_With_Correct_Parameters()
        {
            // Arrange
            var mockRequestService = new Mock<IRequestService>();

            // Create a sample list of elevators.
            var elevators = new List<Elevator>
            {
                new Elevator(1, 5, 10, ElevatorType.Passenger),
                new Elevator(2, 3, 8, ElevatorType.Freight),
                new Elevator(3, 4, 6, ElevatorType.HighSpeed)
            };

            // Create a sample request.
            var request = new Request(3, 2);

            // Setup the DispatchElevator method to simply do nothing.
            mockRequestService.Setup(rs => rs.DispatchElevator(It.IsAny<List<Elevator>>(), It.IsAny<Request>()));

            // Act: Call the DispatchElevator method on the mock.
            mockRequestService.Object.DispatchElevator(elevators, request);

            // Assert: Verify that DispatchElevator was called once with the correct parameters.
            mockRequestService.Verify(rs => rs.DispatchElevator(elevators, request), Times.Once);
        }
    }
}
