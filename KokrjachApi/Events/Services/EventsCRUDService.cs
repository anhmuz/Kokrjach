using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using Protos;
using System.Configuration;

namespace Events
{
    public class EventsCRUDService : EventsCRUD.EventsCRUDBase
    {
        private readonly ILogger<EventsCRUDService> _logger;

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

        private static MySqlConnection GetMySqlConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["mySql"].ConnectionString;
            return new MySqlConnection(connectionString);
        }

        public override Task<GetEventsResponse> GetEvents(Empty request, ServerCallContext context)
        {
            var responce = new GetEventsResponse();
            using MySqlConnection connection = GetMySqlConnection();
            connection.Open();
            var db = new KokrjachEventsDatabase(connection);
            List<KokrjachEventsDatabase.Event> databaseEvents = db.GetEvents().ToList();
            databaseEvents.ForEach(databaseEvent => responce.EventItems.Add(FromDatabaseEvent(databaseEvent)));
            return Task.FromResult(responce);
        }

        public override Task<GetEventResponse> GetEvent(GetEventRequest request, ServerCallContext context)
        {
            using MySqlConnection connection = GetMySqlConnection();
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

        public override Task<AddResponse> Add(AddRequest request, ServerCallContext context)
        {
            using MySqlConnection connection = GetMySqlConnection();
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

        public override Task<Empty> Update(UpdateRequest request, ServerCallContext context)
        {
            int updatedRows;
            using MySqlConnection connection = GetMySqlConnection();
            connection.Open();
            var db = new KokrjachEventsDatabase(connection);
            var update = new KokrjachEventsDatabase.EventUpdate()
            {
                Description = request.EventItemUpdate.Description
            };
            updatedRows = db.Update(request.Id, update);
            if (updatedRows == 0)
            {
                string errorMessage = "Bad ID";
                throw new RpcException(new Status(StatusCode.NotFound, errorMessage));
            }
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
        {
            using MySqlConnection connection = GetMySqlConnection();
            connection.Open();
            var db = new KokrjachEventsDatabase(connection);
            int deletedRows = db.Delete(request.EventItemId);
            if (deletedRows == 0)
            {
                string errorMessage = "Bad ID";
                throw new RpcException(new Status(StatusCode.NotFound, errorMessage));
            }
            return Task.FromResult(new Empty());
        }
    }
}
