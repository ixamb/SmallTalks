using System;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.ChatExchange.Observers;
using TheForge.Services.Delayer;
using TheForge.Services.Views;
using UnityEngine;

namespace SmallTalks.Controllers.InGameNotifications
{
    public class InGameNotificationController : MonoBehaviour, IChatReceivedHandlerWithMetadata
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
            if (ViewService.Instance.GetView("chat-view").IsVisibleAndActive())
            {
                return;
            }
            print("chat received on notification");

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
        }
    }
}