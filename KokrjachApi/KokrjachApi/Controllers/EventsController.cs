using System;
using System.Collections.Generic;
using KokrjachApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

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
            return EventsRepository.Instance.GetEvents();
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
