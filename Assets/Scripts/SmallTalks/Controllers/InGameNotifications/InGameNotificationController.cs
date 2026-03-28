using System;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Chat;
using TheForge.Services.Delayer;
using TheForge.Services.Views;
using UnityEngine;
using VContainer;

namespace SmallTalks.Controllers.InGameNotifications
{
    public sealed class InGameNotificationController : MonoBehaviour
    {
        private IViewService _viewService;
        private IDelayerService _delayerService;
        private IGameDataService _gameDataService;
        private ILocalSaveService _localSaveService;
        private IChatPresenter _chatPresenter;

        private IChatMetadataDispatcher _chatMetadataDispatcher;
        
        [SerializeField] private InGameNotification parentNotification;
        [SerializeField] [Range(0, 10)] private int notificationDuration = 5;

        private const string NotificationDisappearDelayer = "NotificationDisappearDelayer";
        
        [Inject]
        private void Construct(
            IViewService viewService,
            IDelayerService delayerService,
            IGameDataService gameDataService,
            ILocalSaveService localSaveService,
            IChatPresenter chatPresenter,
            IChatMetadataDispatcher chatMetadataDispatcher)
        {
            _viewService = viewService;
            _delayerService = delayerService;
            _gameDataService = gameDataService;
            _localSaveService = localSaveService;
            _chatPresenter = chatPresenter;
            _chatMetadataDispatcher = chatMetadataDispatcher;
        }
        
        private void Start()
        {
            _chatMetadataDispatcher.OnNewChatReceivedWithMetadata += dto => OnNewChatReceivedHandler(dto.NarrativeId, dto.ProfilePicture, dto.Name, dto.Message);
        }

        private void OnNewChatReceivedHandler(Guid narrativeId, Sprite profilePicture, string name, string message)
        {
            if (_chatPresenter.NarrativeId is not null && _chatPresenter.NarrativeId == narrativeId)
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