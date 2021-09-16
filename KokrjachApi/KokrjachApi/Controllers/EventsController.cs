using System;
using System.Collections.Generic;
using Grpc.Net.Client;
using KokrjachApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EventsClient;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace KokrjachApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Event> GetEvents()
        {
            Console.WriteLine("Get a list of events");
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel = GrpcChannel.ForAddress("http://events:80");
            var client = new EventsCRUD.EventsCRUDClient(channel);
            var response = client.GetEvents(new Empty());
            var result = new List<Event>();
            foreach (var eventItem in response.Events)
            {
                Console.WriteLine(string.Format("UserId: {0}, Id: {1}, Description: {2}",
                    eventItem.UserId, eventItem.Id, eventItem.Description));
                var item = new Event {
                    UserId = eventItem.UserId,
                    Id = eventItem.Id,
                    Description = eventItem.Description
                };
                result.Add(item);
            }
            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(int id)
        {
            Console.WriteLine("Get event with id: {0}", id);
            try
            {
                return EventsRepository.Instance.GetEvent(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Event> Post(Event eventItem)
        {
            Console.WriteLine("Post a new event: {0}", eventItem);
            int eventId;
            try
            {
                eventId = EventsRepository.Instance.Add(eventItem);
            }
            catch (ArgumentException)
            {
                return BadRequest(new { Message = "already exists" });
            }
            return CreatedAtAction(nameof(GetEvent), new { id = eventId }, eventItem);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] EventUpdate eventItem)
        {
            Console.WriteLine("Update event with id: {0}", id);
            try
            {
                EventsRepository.Instance.Update(id, eventItem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Event> Delete(int id)
        {
            Console.WriteLine("Delete event with id: {0}", id);
            try
            {
                return EventsRepository.Instance.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
