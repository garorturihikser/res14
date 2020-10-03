using System.Threading.Tasks;

namespace DiscordBot.Backends
{
    public interface IBackend<T>
    {
        public Task Run();

        private Task MessageHandler(T e)
        {
            return Task.CompletedTask;
        }
    }
}