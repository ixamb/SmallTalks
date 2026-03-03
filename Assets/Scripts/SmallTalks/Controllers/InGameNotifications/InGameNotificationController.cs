using System;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Chat;
using TheForge.Services.Delayer;
using TheForge.Services.Views;
using UnityEngine;
using VContainer;

namespace SmallTalks.Controllers.InGameNotifications
{
    public sealed class InGameNotificationController : MonoBehaviour, IChatReceivedHandlerWithMetadata
    {
        private IViewService _viewService;
        private IDelayerService _delayerService;
        private IGameDataService _gameDataService;
        private ILocalSaveService _localSaveService;
        private IChatPresenter _chatPresenter;
        
        [SerializeField] private InGameNotification parentNotification;
        [SerializeField] [Range(0, 10)] private int notificationDuration = 5;

        private const string NotificationDisappearDelayer = "NotificationDisappearDelayer";

        [Inject]
        private void Construct(
            IViewService viewService,
            IDelayerService delayerService,
            IGameDataService gameDataService,
            ILocalSaveService localSaveService,
            IChatPresenter chatPresenter)
        {
            _viewService = viewService;
            _delayerService = delayerService;
            _gameDataService = gameDataService;
            _localSaveService = localSaveService;
            _chatPresenter = chatPresenter;
        }
        
        private void Start()
        {
            ReceivedNewChatMetadataDispatcher.RegisterObserver(this);
        }

        public void OnNewChatReceivedHandler(Guid narrativeId, Sprite profilePicture, string name, string message)
        {
            var chatView = _viewService.GetView<ChatView>();
            if (_chatPresenter.GetNarrativeData() is not null && _chatPresenter.GetNarrativeData()!.Guid == narrativeId)
            {
                return;
            }

            parentNotification.Initialize(profilePicture, name, message);
            parentNotification.OnClick = () => OnNotificationClicked(narrativeId);

            _delayerService.Cancel(NotificationDisappearDelayer);
            _delayerService.Delay(notificationDuration, () => parentNotification.Hide());
            parentNotification.Show();
        }

        private void OnNotificationClicked(Guid narrativeId)
        {
            _delayerService.Cancel(NotificationDisappearDelayer);
            parentNotification.Hide();
            
            var narrative = _gameDataService.GetNarrativeData(narrativeId);
            
            var chatView = _viewService.GetView<ChatView>("chat-view");
            if (!chatView)
                return;

            chatView.Initialize(narrative, _localSaveService.GetNarrativeProgressStep(narrativeId));
            chatView.ShowView();
        }
    }
}