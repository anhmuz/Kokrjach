using System.ComponentModel;

namespace KokrjachApi.Models
{
    public class Event
    {
        [ReadOnly(true)]
        public string UserId { get; set; }

        [ReadOnly(true)]
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
