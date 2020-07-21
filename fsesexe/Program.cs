using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Mono.Options;

using fsesexe.Infrastructure.EventSocket;

namespace fsesexe
{
    class Program
    {
        static int verbosity;

        private static async Task Main(string[] args)
        {
            bool show_help = false;
            List<string> hosts = new List<string>();
            string command = "status";

            var p = new OptionSet() {
                "Usage: greet [OPTIONS]+ message",
                "Greet a list of individuals with an optional message.",
                "If no message is specified, a generic greeting is used.",
                "",
                "Options:",
                { "h|host=", "{HOST} of freeswitch event socket. format) [ipaddress]:[port]:[password]",
                  v => hosts.Add (v) },
                { "x|execute=", "{Execute} command to freeswitch event socket such as 'show calls'",
                  (string v) => command = v },
                { "v", "increase debug message verbosity",
                  v => { if (v != null) ++verbosity; } },
                { "help",  "show this message and exit",
                  v => show_help = v != null },
            };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("greet: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `greet --help' for more information.");
                return;
            }

            if (show_help)
            {
                p.WriteOptionDescriptions(Console.Out);
                return;
            }

            foreach (string host in hosts)
            {
                string ipaddress = "127.0.0.1";
                int port = 8021;
                string password = "ClueCon";

                String pattern = @":";
                String[] elements = System.Text.RegularExpressions.Regex.Split(host, pattern);

                if (elements.Length == 3)
                {
                    ipaddress = elements[0];
                    port = int.Parse(elements[1]);
                    password = elements[2];
                }

                string message = "Connect {0} ...";
                Console.WriteLine(message, ipaddress);

                await EventSocketHandler.SendApi(ipaddress, port, password, command);
            }
        }
    }
}
