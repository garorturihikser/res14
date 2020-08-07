namespace DiscordBot.Config
{
    public interface IConfigGetter
    {
        DiscordBot.Config.Config GetConfig();
    }
}