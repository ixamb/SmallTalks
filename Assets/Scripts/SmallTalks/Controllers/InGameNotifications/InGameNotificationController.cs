using System;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Chat;
using TheForge.Services.Delayer;
using TheForge.Services.Views;
using UnityEngine;

namespace SmallTalks.Controllers.InGameNotifications
{
    public sealed class InGameNotificationController : MonoBehaviour, IChatReceivedHandlerWithMetadata
    {
        [SerializeField] private InGameNotification parentNotification;
        [SerializeField] [Range(0, 10)] private int notificationDuration = 5;

        private const string NotificationDisappearDelayer = "NotificationDisappearDelayer";
        
        private void Start()
        {
            ReceivedChatDispatcherWithMetadata.RegisterObserver(this);
        }

        public void OnNewChatReceivedHandler(Guid narrativeId, Sprite profilePicture, string name, string message)
        {
            var chatView = ViewService.Instance.GetView<ChatView>("chat-view");
            if (chatView.IsVisibleAndActive() || chatView.GetActiveNarrativeId() == narrativeId)
            {
                return;
            }

            parentNotification.Initialize(profilePicture, name, message);
            parentNotification.OnClick = () => OnNotificationClicked(narrativeId);

            ActionDelayerService.Instance.Cancel(NotificationDisappearDelayer);
            ActionDelayerService.Instance.Delay(notificationDuration, () => parentNotification.Hide());
            parentNotification.Show();
        }

        private void OnNotificationClicked(Guid narrativeId)
        {
            ActionDelayerService.Instance.Cancel(NotificationDisappearDelayer);
            parentNotification.Hide();
            
            var narrative = GameDataService.Instance.GetNarrativeData(narrativeId);
            
            var chatView = ViewService.Instance.GetView<ChatView>("chat-view");
            chatView.Initialize(
                senderData: (narrative!.Sender.ProfilePicture, narrative!.Sender.Name),
                narrativeId: narrativeId,
                narrativeEntries: narrative.NarrativeEntries,
                progressStep: LocalSaveService.Instance.GetNarrativeProgressStep(narrativeId));
            chatView.ShowView();
        }
    }
}