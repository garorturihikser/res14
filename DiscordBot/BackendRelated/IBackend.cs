using System;
using System.Threading.Tasks;

namespace DiscordBot.BackendRelated
{
    interface IBackend<T>
    {
        Task Run(Func<IContext, Task> botMessageHandler);

        Task MessageHandler(T e);
    }
}