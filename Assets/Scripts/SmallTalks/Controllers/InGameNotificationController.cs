using System;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.InGameNotifications;
using UnityEngine;

namespace SmallTalks.Controllers
{
    public class InGameNotificationController : MonoBehaviour, IChatReceivedHandlerWithMetadata
    {
        [SerializeField] private InGameNotification parentNotification;

        private void Start()
        {
            ReceivedChatDispatcherWithMetadata.RegisterObserver(this);
        }

        public void OnNewChatReceivedHandler(Guid narrativeId, Sprite profilePicture, string name, string message)
        {
            
        }
    }
}