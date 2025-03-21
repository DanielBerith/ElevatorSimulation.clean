using ElevatorSimulation.Domain.Entities;
using ElevatorSimulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Application.Services
{
    /// <summary>
    /// Manages a queue of elevator requests.
    /// </summary>
    public class RequestQueue
    {
        private readonly List<Request> _requestQueue = new List<Request>();

        /// <summary>
        /// Enqueues a new elevator request.
        /// </summary>
        public void EnqueueRequest(Request request)
        {
            _requestQueue.Add(request);
        }

        /// <summary>
        /// Dequeues the nearest request based on the elevator's current floor and direction.
        /// When idle, returns the nearest request regardless of direction.
        /// </summary>
        public Request DequeueRequest(int currentFloor, Direction currentDirection)
        {
            if (_requestQueue.Count == 0)
                return null;

            // Sort by proximity.
            List<Request> sortedRequests = _requestQueue
                .OrderBy(r => Math.Abs(r.Floor - currentFloor))
                .ToList();

            Request selectedRequest = null;
            if (currentDirection == Direction.Idle)
            {
                selectedRequest = sortedRequests.FirstOrDefault();
            }
            else if (currentDirection == Direction.Up)
            {
                selectedRequest = sortedRequests.FirstOrDefault(r => r.Floor >= currentFloor);
            }
            else if (currentDirection == Direction.Down)
            {
                selectedRequest = sortedRequests.LastOrDefault(r => r.Floor <= currentFloor);
            }

            if (selectedRequest != null)
                _requestQueue.Remove(selectedRequest);

            return selectedRequest;
        }
    }
}
