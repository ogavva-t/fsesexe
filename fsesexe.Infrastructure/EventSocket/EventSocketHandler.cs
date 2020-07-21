using System;
using System.Threading.Tasks;
using NEventSocket;

namespace fsesexe.Infrastructure.EventSocket
{
    public static class EventSocketHandler
    {
        public  static async Task SendApi(string ipaddress, int port, string password, string command)
        {
            using (var socket = await InboundSocket.Connect(ipaddress, port, password))
            {
                var apiResponse = await socket.SendApi(command);
                Console.WriteLine(apiResponse.BodyText);
            }
            return;
        }
    }
}
