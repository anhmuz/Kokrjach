using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using KokrjachApi.Models;

namespace KokrjachApi
{
    class EventsRepository
    {
        private Dictionary<int,Event> _events = new Dictionary<int, Event>();
        private int _eventId = 1;

        private EventsRepository()
        {
        }

        public static EventsRepository Instance { get; } = new EventsRepository();

        public IEnumerable<Event> GetEvents()
        {
            return _events.Values;
        }

        public IEnumerable<Event> GetEventsByTypeId(string eventTypeId)
        {
            return _events.Where(item => item.Value.EventTypeId == eventTypeId).
                ToDictionary(x => x.Key, x => x.Value).Values;
        }

        public Event GetEvent(int id)
        {
            return _events[id];
        }

        public int Add(Event eventItem)
        {
            int currentId = _eventId;
            _events.Add(currentId, eventItem);
            ++_eventId;
            return currentId;
        }

        public void Update(int id, Event eventItem)
        {
            if (!_events.ContainsKey(id))
            {
                throw new KeyNotFoundException();
            }
            _events[id] = eventItem;
        }

        public Event Delete(int id)
        {
            var removedEvent = _events[id];
            _events.Remove(id);
            return removedEvent;
        }
    }
}
