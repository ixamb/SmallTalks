using System;
using System.Collections.Generic;
using SmallTalks.Extensions;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.LocalSave;
using TheForge.Services;
using TheForge.Services.Delayer;
using Random = UnityEngine.Random;

namespace SmallTalks.Services.ChatExchange
{
    public sealed class ChatExchangeService : Singleton<ChatExchangeService, IChatExchangeService>, IChatExchangeService
    {
        private readonly List<IChatReceivedHandler> _chatReceivedHandlers = new();
        
        protected override void Init() { }

        public void SendMessage(Guid narrativeId)
        {
            LocalSaveService.Instance.IncreaseNarrativeProgressStep(narrativeId, 1);
            ExpectSenderAnswer(narrativeId);
        }
        
        public void ExpectSenderAnswer(Guid narrativeId, bool isFirstMessage = false)
        {
            if (isFirstMessage)
            {
                LocalSaveService.Instance.IncreaseNarrativeProgressStep(narrativeId, -1, autoSave: false);
            }
            
            WaitForAnswer(onWait: () =>
            {
                ReceiveAnswer(narrativeId);
            });
        }

        private void WaitForAnswer(Action onWait)
        {
            var waitInSeconds = Random.Range(2, 5f);
            ActionDelayerService.Instance.Delay(waitInSeconds, onWait);
        }

        private void ReceiveAnswer(Guid narrativeId)
        {
            LocalSaveService.Instance.IncreaseNarrativeProgressStep(narrativeId, 1, autoSave: false);
            if (!LocalSaveService.Instance.TryGetNarrativeProgressStep(narrativeId, out var progressStep))
                return;
            
            LocalSaveService.Instance.MarkNarrativeProgressNewMessageStatus(narrativeId, true, autoSave: false);
            LocalSaveService.Instance.Save();
            _chatReceivedHandlers.ForEach(h => h.OnNewChatReceivedHandler(narrativeId, progressStep!.Value));
        }

        public void RegisterChatReceivedHandler(IChatReceivedHandler handler) => _chatReceivedHandlers.AddUnique(handler);
        public void UnregisterChatReceivedHandler(IChatReceivedHandler handler) => _chatReceivedHandlers.RemoveUnique(handler);
    }
}