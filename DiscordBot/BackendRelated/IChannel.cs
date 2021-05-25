namespace DiscordBot.BackendRelated
{
    public interface IChannel<T>
    {
        T GetChannel();
    }
}