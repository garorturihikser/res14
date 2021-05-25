namespace DiscordBot.BackendRelated
{
    public interface IUser<T>
    {
        T GetUser();
    }
}