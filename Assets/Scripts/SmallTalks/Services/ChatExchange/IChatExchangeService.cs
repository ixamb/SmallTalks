using System;
using SmallTalks.Services.ChatExchange.Observers;
using TheForge.Services;

namespace SmallTalks.Services.ChatExchange
{
    public interface IChatExchangeService
    {
        void SendMessage(Guid narrativeId);
        void ExpectSenderAnswer(Guid narrativeId, bool isFirstMessage = false);

        void RegisterChatReceivedHandler(IChatReceivedHandler handler);
        void UnregisterChatReceivedHandler(IChatReceivedHandler handler);
    }
}