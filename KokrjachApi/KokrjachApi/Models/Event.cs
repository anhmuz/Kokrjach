using System.ComponentModel;

namespace KokrjachApi.Models
{
    public class Event
    {
        [ReadOnly(true)]
        public string UserId { get; set; }
        public int Id { get; set; }
        public string EventTypeId { get; set; }
        public string Description { get; set; }
    }
}
