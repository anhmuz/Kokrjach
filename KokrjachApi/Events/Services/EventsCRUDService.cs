using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf.Collections;

namespace Events
{
    public class EventsCRUDService : EventsCRUD.EventsCRUDBase
    {
        private readonly ILogger<EventsCRUDService> _logger;
        public EventsCRUDService(ILogger<EventsCRUDService> logger)
        {
            _logger = logger;
        }

        public override Task<GetEventsResponse> GetEvents(Empty request, ServerCallContext context)
        {
            var event1 = new EventItem
            {
                UserId = "Vadym",
                Id = 1,
                Description = "aaa"
            };
            var event2 = new EventItem
            {
                UserId = "Anhelina",
                Id = 2,
                Description = "bbb"
            };

            //var events = new RepeatedField<Event>();
            //events.Add(event1);
            //events.Add(event2);

            var events = new[] { event1, event2 };

            var responce = new GetEventsResponse();
            responce.Events.Add(events);

            return Task.FromResult(responce);
        }
    }
}
