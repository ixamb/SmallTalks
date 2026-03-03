using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.NarrativeList;
using SmallTalks.UI.Swipe;
using TheForge.Services.Delayer;
using TheForge.Services.Scenes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SmallTalks.Services
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [Header("Service configurations")]
        [SerializeField] private GameDataContainer gameDataContainer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISceneService, SceneService>(Lifetime.Singleton);
            builder.Register<TheForge.Services.LocalSave.ILocalSaveService, TheForge.Services.LocalSave.LocalSaveService>(Lifetime.Singleton);
            builder.Register<ILocalSaveService, LocalSaveService>(Lifetime.Singleton);
            builder.Register<IGameDataService, GameDataService>(Lifetime.Singleton).WithParameter(gameDataContainer);
            builder.Register<IDelayerService, DelayerService>(Lifetime.Singleton);
            builder.Register<IChatExchangeService, ChatExchangeService>(Lifetime.Singleton);
            
            builder.Register<INarrativeStackManager, NarrativeStackManager>(Lifetime.Singleton);
            builder.Register<INarrativePreviewListPresenter, NarrativePreviewListPresenter>(Lifetime.Singleton);
        }
    }
}