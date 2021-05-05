using System.Collections.Generic;
using System.Linq;
using KokrjachApi.Models;

namespace KokrjachApi
{
    class EventsRepository
    {
        private readonly List<Event> _events = new List<Event>();

        private EventsRepository()
        {
        }

        public static EventsRepository Instance { get; } = new EventsRepository();

        public IEnumerable<Event> GetEvents()
        {
            return _events;
        }

        public IEnumerable<Event> GetEventsByTypeId(string eventTypeId)
        {
            return _events.Where(item => item.EventTypeId == eventTypeId);
        }

        public Event GetEvent(int id)
        {
            return _events[id - 1];
        }

        public int Add(Event eventItem)
        {
            _events.Add(eventItem);
            return _events.Count;
        }
    }
}
