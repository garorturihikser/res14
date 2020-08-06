using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    static class CommandParser
    {
        public static string[] Parse(DiscordMessage msg, string commandPrefix)
        {
            string[] content = msg.Content.ToLower().Split();
            string curCommand = content[0];
            curCommand = curCommand.Replace(commandPrefix, "");
            return content;
        }
    }
}
