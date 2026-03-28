using SmallTalks.Services.ChatExchange;
using TheForge.Services.Views;
using VContainer;
using VContainer.Unity;

namespace SmallTalks.Services
{
    public sealed class GameObjectLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IViewService, ViewService>(Lifetime.Singleton);
            builder.Register<IChatMetadataDispatcher, ChatMetadataDispatcher>(Lifetime.Singleton);
        }
    }
}