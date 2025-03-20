using ElevatorSimulation.Domain.Entities;
using ElevatorSimulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Services
{
    public class RequestQueue
    {
        private List<Request> _requestQueue = new List<Request>();

        // Enqueue a new request
        public void EnqueueRequest(Request request)
        {
            _requestQueue.Add(request);
        }

        // Dequeue the nearest request based on elevator's current position and direction
        public Request DequeueRequest(int currentFloor, Direction currentDirection)
        {
            // Sort requests based on direction
            List<Request> sortedRequests = _requestQueue
                .OrderBy(r => Math.Abs(r.Floor - currentFloor)) // Sort by proximity to current floor
                .ToList();

            // First request decides the direction
            if (currentDirection == Direction.Up)
            {
                // Prioritize requests that are above the current floor
                return sortedRequests.FirstOrDefault(r => r.Floor >= currentFloor);
            }
            else if (currentDirection == Direction.Down)
            {
                // Prioritize requests that are below the current floor
                return sortedRequests.LastOrDefault(r => r.Floor <= currentFloor);
            }

            return null;
        }

    }
}
