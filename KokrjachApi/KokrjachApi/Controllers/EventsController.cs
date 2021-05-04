using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private static List<Event> _events = new List<Event>()
        {
            new Event()
            {
                UserId = "Vadym",
                EventTypeId = "Сash withdrawal",
                Description = "Vadym withdrew cash"
            },
            new Event()
            {
                UserId = "Anhelina",
                EventTypeId = "Money transfer",
                Description = "Anhelina transferred money"
            },
            new Event()
            {
                UserId = "Wolter",
                EventTypeId = "Сash withdrawal",
                Description = "Wolter withdrew cash"
            },
            new Event()
            {
                UserId = "Kokrjach",
                EventTypeId = "Money transfer",
                Description = "Kokrjach transferred money"
            }
        };

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
                return _events;
            }
            return _events.Where(item => item.EventTypeId == eventTypeId.ToString());
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(int id)
        {
            return _events[id - 1];
        }

        [HttpPost]
        public ActionResult<Event> Post(Event eventItem)
        {
            _events.Add(eventItem);
            Console.WriteLine("Post a new event: {0}", eventItem);
            return CreatedAtAction(nameof(GetEvent), new { id = _events.Count }, eventItem);
        }
    }
}
