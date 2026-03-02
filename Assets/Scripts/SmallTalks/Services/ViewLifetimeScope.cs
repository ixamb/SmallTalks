using TheForge.Services.Views;
using VContainer;
using VContainer.Unity;

namespace SmallTalks.Services
{
    public sealed class ViewLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IViewService, ViewService>(Lifetime.Singleton);
        }
    }
}