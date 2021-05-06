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
        private List<Event> _events = new List<Event>();

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

        public void Update(int id, Event eventItem)
        {
            _events[id - 1] = eventItem;
        }

        public Event Delete(int id)
        {
            var removedEvent = _events[id - 1];
            _events.RemoveAt(id - 1);
            return removedEvent;
        }
    }
}
