using System;

namespace SmallTalks.Services.ChatExchange.Observers
{
    public interface IChatReceivedHandler
    {
        void OnNewChatReceivedHandler(Guid narrativeId, int progressStep);
    }
}