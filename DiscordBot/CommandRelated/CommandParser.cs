namespace DiscordBot
{
    static class CommandParser
    {
        public static string[] Parse(string content, string commandPrefix)
        {
            string[] splitContent = Split(content);
            splitContent[0] = RemovePrefix(splitContent[0], commandPrefix);
            return splitContent;
        }

        public static string[] Split(string content)
        {
            return content.ToLower().Split();
        }

        private static string RemovePrefix(string content, string commandPrefix)
        {
            content = content.Replace(commandPrefix, "");
            return content;
        }
    }
}
