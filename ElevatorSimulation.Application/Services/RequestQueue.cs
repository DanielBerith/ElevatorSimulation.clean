using ElevatorSimulation.Domain.Entities;
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

        // Dequeue the nearest request based on elevator's current position
        public Request DequeueNearestRequest(int currentFloor)
        {
            if (_requestQueue.Count == 0)
                return null;

            // Sort the requests by the absolute difference between current floor and requested floor
            var nearestRequest = _requestQueue
                .OrderBy(r => Math.Abs(r.Floor - currentFloor)) // Sort by the nearest floor
                .FirstOrDefault(); // Get the nearest request

            // Remove the selected request from the list
            if (nearestRequest != null)
            {
                _requestQueue.Remove(nearestRequest);
            }

            return nearestRequest;
        }

    }
}
