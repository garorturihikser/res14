namespace DiscordBot.BackendRelated
{
    public interface IMessage
    {
        System.Type AuthorType { get; set; }
        System.Type ChannelType { get; set; }
    }
}