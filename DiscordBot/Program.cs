using DiscordBot.Commands;
using DiscordBot.Configurations;

namespace DiscordBot
{
    class Program
    {

        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.Run(args, ConfigBot.GetConfigurations()).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
