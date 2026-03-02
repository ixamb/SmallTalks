using System;
using System.Collections.Generic;
using SmallTalks.Extensions;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.LocalSave;
using TheForge.Services.Delayer;
using Random = UnityEngine.Random;

namespace SmallTalks.Services.ChatExchange
{
    public sealed class ChatExchangeService : IChatExchangeService
    {
        private ILocalSaveService _localSaveService;
        private IDelayerService _delayerService;
        
        private readonly List<IChatReceivedHandler> _chatReceivedHandlers = new();

        public ChatExchangeService(ILocalSaveService localSaveService, IDelayerService delayerService)
        {
            _localSaveService = localSaveService;
            _delayerService = delayerService;
        }
        
        public void SendMessage(Guid narrativeId)
        {
            _localSaveService.IncreaseNarrativeProgressStep(narrativeId, 1);
            ExpectSenderAnswer(narrativeId);
        }
        
        public void ExpectSenderAnswer(Guid narrativeId, bool isFirstMessage = false)
        {
            if (isFirstMessage)
            {
                _localSaveService.IncreaseNarrativeProgressStep(narrativeId, -1, autoSave: false);
            }
            
            WaitForAnswer(onWait: () =>
            {
                ReceiveAnswer(narrativeId);
            });
        }

        private void WaitForAnswer(Action onWait)
        {
            var waitInSeconds = Random.Range(2, 5f);
            _delayerService.Delay(waitInSeconds, onWait);
        }

        private void ReceiveAnswer(Guid narrativeId)
        {
            _localSaveService.IncreaseNarrativeProgressStep(narrativeId, 1, autoSave: false);
            if (!_localSaveService.TryGetNarrativeProgressStep(narrativeId, out var progressStep))
                return;
            
            _localSaveService.MarkNarrativeProgressNewMessageStatus(narrativeId, true, autoSave: false);
            _localSaveService.Save();
            _chatReceivedHandlers.ForEach(h => h.OnNewChatReceivedHandler(narrativeId, progressStep!.Value));
        }

        public void RegisterChatReceivedHandler(IChatReceivedHandler handler) => _chatReceivedHandlers.AddUnique(handler);
        public void UnregisterChatReceivedHandler(IChatReceivedHandler handler) => _chatReceivedHandlers.RemoveUnique(handler);
    }
}