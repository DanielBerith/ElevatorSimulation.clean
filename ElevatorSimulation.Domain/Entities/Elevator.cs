﻿using ElevatorSimulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Domain.Entities
{
    public class Elevator
    {
        public int Id { get; set; }
        public int CurrentFloor { get; private set; }
        public Direction Direction { get; private set; }
        public int Occupants { get; private set; }
        public int MaxCapacity { get; private set; }
        public ElevatorType Type { get; private set; }

        public event Action<Elevator> ElevatorStatusChanged;

        public Elevator(int id, int initialFloor, int maxCapacity, ElevatorType type)
        {
            Id = id;
            CurrentFloor = initialFloor;
            MaxCapacity = maxCapacity;
            Type = type;
            Direction = Direction.Idle;
        }

        // Getters for state
        public int GetCurrentFloor() => CurrentFloor;
        public Direction GetDirection() => Direction;
        public int GetOccupants() => Occupants;
        public int GetMaxCapacity() => MaxCapacity;
        protected virtual void OnElevatorStatusChanged()
        {
            ElevatorStatusChanged?.Invoke(this);
        }

        public (int boarded, int left) BoardPassengers(int people)
        {
            int available = MaxCapacity - Occupants;
            int toBoard = Math.Min(people, available);
            Occupants += toBoard;
            OnElevatorStatusChanged();
            return (toBoard, people - toBoard);
        }

        public virtual void MoveToFloor(int targetFloor)
        {
            if (CurrentFloor < targetFloor)
                Direction = Direction.Up;
            else if (CurrentFloor > targetFloor)
                Direction = Direction.Down;
            else
                Direction = Direction.Idle;

            Console.WriteLine($"Elevator {Id} moving {Direction} to floor {targetFloor}");
            CurrentFloor = targetFloor;
            Direction = Direction.Idle;
        }
    }
}
