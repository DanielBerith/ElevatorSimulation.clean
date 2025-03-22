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
    public class FloorBasedDispatchingStrategyTests
    {
        [Fact]
        public void DispatchElevator_ForFloor3_Returns_FreightElevator()
        {
            // Arrange: Create a list of elevators with different types.
            var elevators = new List<Elevator>
            {
                new Elevator(1, 5, 10, ElevatorType.Passenger),  // Passenger elevator at floor 5
                new Elevator(2, 4, 10, ElevatorType.Freight),     // Freight elevator at floor 4
                new Elevator(3, 7, 10, ElevatorType.HighSpeed)    // HighSpeed elevator at floor 7
            };

            IDispatchingStrategy strategy = new FloorBasedDispatchingStrategy();
            int requestedFloor = 3; // Should trigger a Freight elevator

            // Act
            var selected = strategy.DispatchElevator(elevators, requestedFloor);

            // Assert: The selected elevator should be of type Freight.
            Assert.NotNull(selected);
            Assert.Equal(ElevatorType.Freight, selected.Type);
        }

        [Fact]
        public void DispatchElevator_ForFloor1_Returns_PassengerElevator()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1, 2, 10, ElevatorType.Passenger), // Passenger elevator at floor 2
                new Elevator(2, 5, 10, ElevatorType.Passenger), // Passenger elevator at floor 5
                new Elevator(3, 3, 10, ElevatorType.Freight)    // Freight elevator at floor 3
            };

            IDispatchingStrategy strategy = new FloorBasedDispatchingStrategy();
            int requestedFloor = 1; // Should trigger a Passenger elevator

            // Act
            var selected = strategy.DispatchElevator(elevators, requestedFloor);

            // Assert: The selected elevator should be of type Passenger.
            Assert.NotNull(selected);
            Assert.Equal(ElevatorType.Passenger, selected.Type);
        }

        [Fact]
        public void DispatchElevator_ForFloor6_Returns_HighSpeedElevator()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1, 2, 10, ElevatorType.Passenger), // Passenger elevator
                new Elevator(2, 4, 10, ElevatorType.Freight),    // Freight elevator
                new Elevator(3, 3, 10, ElevatorType.HighSpeed)   // HighSpeed elevator
            };

            IDispatchingStrategy strategy = new FloorBasedDispatchingStrategy();
            int requestedFloor = 6; // Should trigger a HighSpeed elevator

            // Act
            var selected = strategy.DispatchElevator(elevators, requestedFloor);

            // Assert: The selected elevator should be of type HighSpeed.
            Assert.NotNull(selected);
            Assert.Equal(ElevatorType.HighSpeed, selected.Type);
        }

        [Fact]
        public void DispatchElevator_ForOtherFloors_DefaultsToPassengerElevator()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new Elevator(1, 4, 10, ElevatorType.Passenger), // Passenger elevator
                new Elevator(2, 8, 10, ElevatorType.Freight),    // Freight elevator
                new Elevator(3, 6, 10, ElevatorType.HighSpeed)   // HighSpeed elevator
            };

            IDispatchingStrategy strategy = new FloorBasedDispatchingStrategy();
            int requestedFloor = 5; // Not specifically handled, should default to Passenger

            // Act
            var selected = strategy.DispatchElevator(elevators, requestedFloor);

            // Assert: The selected elevator should be of type Passenger.
            Assert.NotNull(selected);
            Assert.Equal(ElevatorType.Passenger, selected.Type);
        }
    }

}
