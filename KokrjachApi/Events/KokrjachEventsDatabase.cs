using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Events
{
    public class KokrjachEventsDatabase
    {
        private readonly MySqlConnection _connection;
        public class Event
        {
            public string UserId { get; set; }
            public int Id { get; set; }
            public string Description { get; set; }
            public override string ToString()
            {
                return string.Format("UserId: {0}, Id: {1}, Description: {2}", UserId, Id, Description);
            }
        }

        public class EventUpdate
        {
            public string Description { get; set; }
        }

        public KokrjachEventsDatabase(MySqlConnection sqlConnection)
        {
            _connection = sqlConnection;
        }

        public IEnumerable<Event> GetEvents()
        {
            string sql = @"select user_id, id, event_description from event_item;";
            var result = new List<Event>();
            using (MySqlCommand command = new MySqlCommand(sql, _connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Event() {
                            UserId = reader.GetString("user_id"),
                            Id = reader.GetInt32("id"),
                            Description = reader.GetString("event_description")
                        });
                    }
                }
            }
            return result;
        }

        public Event GetEvent(int id)
        {
            string sql = @"select user_id, id, event_description from event_item
where id=@id;";
            using (MySqlCommand command = new MySqlCommand(sql, _connection))
            {
                command.Parameters.Add(new MySqlParameter("@id", id));
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Event()
                        {
                            UserId = reader.GetString("user_id"),
                            Id = reader.GetInt32("id"),
                            Description = reader.GetString("event_description")
                        };
                    }
                }
            }
            return null;
        }

        public int Add(Event databaseEvent)
        {
            string sql = @"insert into event_item
(user_id, event_description)
values (@userId, @description);";
            int id;
            using (MySqlCommand command = new MySqlCommand(sql, _connection))
            {
                command.Parameters.Add(new MySqlParameter("@userId", databaseEvent.UserId));
                command.Parameters.Add(new MySqlParameter("@description", databaseEvent.Description));
                command.ExecuteNonQuery();
                id = (int)command.LastInsertedId;
            }
            return id;
        }

        public int Update(int id, EventUpdate databaseEventUpdate)
        {
            string sql = @"update event_item
set event_description=@description
where id=@id;";
            int rowsAffected;
            using (MySqlCommand command = new MySqlCommand(sql, _connection))
            {
                command.Parameters.Add(new MySqlParameter("@description", databaseEventUpdate.Description));
                command.Parameters.Add(new MySqlParameter("@id", id));
                rowsAffected  = command.ExecuteNonQuery();
            }
            return rowsAffected;
        }
        
        public void Delete(int id)
        {
            string sql = @"delete from event_item where id=@id;";
            using (MySqlCommand command = new MySqlCommand(sql, _connection))
            {
                command.Parameters.Add(new MySqlParameter("@id", id));
                command.ExecuteNonQuery();
            }
        }
    }
}
