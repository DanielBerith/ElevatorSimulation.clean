using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Application.Services;
using ElevatorSimulation.Domain.Entities;
using ElevatorSimulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.UnitTests.Features.DispatchingStrategies
{
    public class NearestElevatorStrategyTests
    {
        [Fact]
        public void DispatchElevator_Returns_Nearest_Elevator()
        {
            // Arrange: Create a sample list of elevators.
            var elevators = new List<Elevator>
            {
                new Elevator(1, 1, 10, ElevatorType.Passenger),
                new Elevator(2, 3, 10, ElevatorType.Passenger),
                new Elevator(3, 6, 10, ElevatorType.Passenger)
            };

            // Instantiate the strategy.
            IDispatchingStrategy strategy = new NearestElevatorStrategy();

            // Act: Request an elevator for floor 4.
            Elevator selected = strategy.DispatchElevator(elevators, 4);

            // Assert: Elevator 2 (at floor 3) should be the nearest to floor 4.
            Assert.NotNull(selected);
            Assert.Equal(2, selected.Id);
        }
    }
}
