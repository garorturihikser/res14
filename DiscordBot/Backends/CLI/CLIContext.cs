using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.Backends.CLI
{
    public class CLIContext: IContext
    {
        public string Msg { get; set; }

        public CLIContext(string e)
        {
            Msg = e;
        }

        public DiscordChannel GetChannel() =>
            null;

        public DiscordUser GetSender() =>
            null;

        public string ExtractMessageContent()
        {
            return Msg;
        }
        
        public async Task Respond(string content)
        {
            Console.WriteLine(content);
        }
    }
}