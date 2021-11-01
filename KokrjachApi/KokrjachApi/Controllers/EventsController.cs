using System;
using System.Collections.Generic;
using Grpc.Net.Client;
using KokrjachApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EventsClient;
using Google.Protobuf.WellKnownTypes;
using System.Net.Http;
using System.Linq;

namespace KokrjachApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly GrpcChannel _channel;

        private Event FromEventItem(EventItem eventItem)
        {
            return new Event
            {
                UserId = eventItem.UserId,
                Id = eventItem.Id,
                Description = eventItem.Description
            };
        }

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            _channel = GrpcChannel.ForAddress("https://events:443", new GrpcChannelOptions { HttpHandler = httpHandler });
        }

        [HttpGet]
        public IEnumerable<Event> GetEvents()
        {
            Console.WriteLine("Get a list of events");
            var client = new EventsCRUD.EventsCRUDClient(_channel);
            GetEventsResponse response = client.GetEvents(new Empty());
            var result = new List<Event>();
            response.EventItems.ToList().ForEach(eventItem => result.Add(FromEventItem(eventItem)));
            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(int id)
        {
            Console.WriteLine("Get event with id: {0}", id);
            var client = new EventsCRUD.EventsCRUDClient(_channel);
            var request = new GetEventRequest()
            {
                EventItemId = id
            };
            GetEventResponse response = client.GetEvent(request);
            if (response.EventItem == null)
            {
                return NotFound();
            }
            return FromEventItem(response.EventItem);
        }

        [HttpPost]
        public ActionResult<Event> Post(Event eventAdd)
        {
            Console.WriteLine("Post a new event: {0}", eventAdd);
            var client = new EventsCRUD.EventsCRUDClient(_channel);
            var eventItem = new EventItem()
            {
                UserId = eventAdd.UserId,
                Description = eventAdd.Description
            };
            var request = new AddRequest()
            {
                EventItem = eventItem
            };
            AddResponse response = client.Add(request);
            return CreatedAtAction(nameof(GetEvent), new { id = response.EventItemId }, eventAdd);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] EventUpdate eventUpdate)
        {
            Console.WriteLine("Update event with id: {0}", id);
            var client = new EventsCRUD.EventsCRUDClient(_channel);
            var eventItem = new EventItem()
            {
                Id = id,
                Description = eventUpdate.Description
            };
            var request = new UpdateRequest()
            {
                EventItem = eventItem
            };
            UpdateResponse response = client.Update(request);
            if (response.EventItem == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Event> Delete(int id)
        {
            Console.WriteLine("Delete event with id: {0}", id);
            var client = new EventsCRUD.EventsCRUDClient(_channel);
            var request = new DeleteRequest()
            {
                EventItemId = id
            };
            DeleteResponse response = client.Delete(request);
            if (response.EventItem == null)
            {
                return NotFound();
            }
            return FromEventItem(response.EventItem);
        }
    }
}
