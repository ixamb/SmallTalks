using System;

namespace SmallTalks.Services.ChatExchange.Observers
{
    public interface INewChatMessageObserver
    {
        void OnNewChatMessageReceived(Guid narrativeId, int progressStep);
    }
}