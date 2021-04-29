using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokrjachApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            var result = new List<Event>();
            result.Add(new Event()
            {
                UserId = "Vadym",
                EventTypeId = "Сash withdrawal",
                Description = "Vadym withdrew cash"
            });
            result.Add(new Event()
            {
                UserId = "Anhelina",
                EventTypeId = "Money transfer",
                Description = "Anhelina transferred money"
            });
            Console.WriteLine("Get a list of events");
            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(long id)
        {
            return new Event()
            {
                UserId = "Vadym",
                EventTypeId = "Сash withdrawal",
                Description = "Vadym withdrew cash"
            };
        }

        [HttpPost]
        public ActionResult<Event> Post(Event eventItem)
        {
            Console.WriteLine("Post a new event: {0}", eventItem);
            return CreatedAtAction(nameof(GetEvent), new { id = "1" }, eventItem);
        }
    }
}
