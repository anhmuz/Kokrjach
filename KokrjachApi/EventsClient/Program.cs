using System;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using Protos;

namespace EventsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new EventsCRUD.EventsCRUDClient(channel);
            var response = client.GetEvents(new Empty());
            foreach (var eventItem in response.EventItems)
            {
                Console.WriteLine(string.Format("UserId: {0}, Id: {1}, Description: {2}",
                    eventItem.UserId, eventItem.Id, eventItem.Description));
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}