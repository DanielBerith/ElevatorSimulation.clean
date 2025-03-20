using ElevatorSimulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Interfaces
{
    public interface IRequestService
    {
        void DispatchElevator(List<Elevator> elevators, Request request);
    }
}
