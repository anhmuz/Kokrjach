using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Events
{
    public class EventsCRUDService : EventsCRUD.EventsCRUDBase
    {
        private readonly ILogger<EventsCRUDService> _logger;

        private readonly MySqlConnectionStringBuilder _builder = new MySqlConnectionStringBuilder
        {
            Server = "database",
            Database = "kokrjach_events",
            UserID = "root",
            Password = "root",
            Port = 3306
        };
        public EventsCRUDService(ILogger<EventsCRUDService> logger)
        {
            _logger = logger;
        }

        private EventItem FromDatabaseEvent(KokrjachEventsDatabase.Event databaseEvent)
        {
            return new EventItem
            {
                UserId = databaseEvent.UserId,
                Id = databaseEvent.Id,
                Description = databaseEvent.Description
            };
        }

        public override Task<GetEventsResponse> GetEvents(Empty request, ServerCallContext context)
        {
            var responce = new GetEventsResponse();
            using (MySqlConnection connection = new MySqlConnection(_builder.ToString()))
            {
                connection.Open();
                var db = new KokrjachEventsDatabase(connection);
                List<KokrjachEventsDatabase.Event> databaseEvents = db.GetEvents().ToList();
                databaseEvents.ForEach(databaseEvent => responce.EventItems.Add(FromDatabaseEvent(databaseEvent)));
                return Task.FromResult(responce);
            }
        }

        public override Task<GetEventResponse> GetEvent(GetEventRequest request, ServerCallContext context)
        {
            using (MySqlConnection connection = new MySqlConnection(_builder.ToString()))
            {
                connection.Open();
                var db = new KokrjachEventsDatabase(connection);
                KokrjachEventsDatabase.Event databaseEvent = db.GetEvent(request.EventItemId);
                if (databaseEvent == null)
                {
                    return Task.FromResult(new GetEventResponse());
                }
                var response = new GetEventResponse()
                {
                    EventItem = FromDatabaseEvent(databaseEvent)
                };
                return Task.FromResult(response);
            }
        }

        public override Task<AddResponse> Add(AddRequest request, ServerCallContext context)
        {
            using (MySqlConnection connection = new MySqlConnection(_builder.ToString()))
            {
                connection.Open();
                var db = new KokrjachEventsDatabase(connection);
                var databaseEvent = new KokrjachEventsDatabase.Event()
                {
                    UserId = request.EventItem.UserId,
                    Description = request.EventItem.Description
                };
                int id = db.Add(databaseEvent);
                var response = new AddResponse()
                {
                    EventItemId = id
                };
                return Task.FromResult(response);
            }
        }

        public override Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
        {
            int updatedRows;
            using (MySqlConnection connection = new MySqlConnection(_builder.ToString()))
            {
                connection.Open();
                var db = new KokrjachEventsDatabase(connection);
                var databaseEvent = new KokrjachEventsDatabase.Event()
                {
                    Id = request.EventItem.Id,
                    Description = request.EventItem.Description
                };
                updatedRows = db.Update(databaseEvent);
                if (updatedRows == 0)
                {
                    return Task.FromResult(new UpdateResponse());
                }
                EventItem eventItem = FromDatabaseEvent(db.GetEvent(request.EventItem.Id));
                var response = new UpdateResponse()
                {
                    EventItem = eventItem
                };
                return Task.FromResult(response);
            }
        }
        public override Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
        {
            using (MySqlConnection connection = new MySqlConnection(_builder.ToString()))
            {
                connection.Open();
                var db = new KokrjachEventsDatabase(connection);
                KokrjachEventsDatabase.Event databaseEvent = db.GetEvent(request.EventItemId);
                if (databaseEvent == null)
                {
                    return Task.FromResult(new DeleteResponse());
                }
                db.Delete(request.EventItemId);
                var response = new DeleteResponse()
                {
                    EventItem = FromDatabaseEvent(databaseEvent)
                };
                return Task.FromResult(response);
            }
        }
    }
}
