using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.BackendRelated.CLI
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
        
        public Task Respond(string content)
        {
            Console.WriteLine(content);
            return Task.CompletedTask;
        }
    }
}