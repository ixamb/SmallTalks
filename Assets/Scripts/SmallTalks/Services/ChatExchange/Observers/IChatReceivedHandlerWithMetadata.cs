using System;
using UnityEngine;

namespace SmallTalks.Services.ChatExchange.Observers
{
    public interface IChatReceivedHandlerWithMetadata
    {
        void OnNewChatReceivedHandler(Guid narrativeId, Sprite profilePicture, string name, string message);
    }
}