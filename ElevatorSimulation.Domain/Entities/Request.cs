using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Domain.Entities
{
    public class Request
    {
        public int Floor { get; }
        public int Passengers { get; }

        public Request(int floor, int passengers)
        {
            Floor = floor;
            Passengers = passengers;
        }
    }
}
