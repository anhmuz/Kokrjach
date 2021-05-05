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
        private EventsRepository _eventsRepository = EventsRepository.Instance;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Event> GetEvents()
        {
            Console.WriteLine("Get a list of events");
            StringValues eventTypeId;
            bool hasEventTypeIdParam = HttpContext.Request.Query.TryGetValue(nameof(Event.EventTypeId), out eventTypeId);
            if (!hasEventTypeIdParam)
            {
                return _eventsRepository.GetEvents();
            }
            return _eventsRepository.GetEventsByTypeId(eventTypeId.ToString());
        }

        [HttpGet("{id}")]
        public Event GetEvent(int id)
        {
            return _eventsRepository.GetEvent(id);
        }

        [HttpPost]
        public ActionResult<Event> Post(Event eventItem)
        {
            Console.WriteLine("Post a new event: {0}", eventItem);
            int eventId = _eventsRepository.Add(eventItem);
            return CreatedAtAction(nameof(GetEvent), new { id = eventId }, eventItem);
        }
    }
}
