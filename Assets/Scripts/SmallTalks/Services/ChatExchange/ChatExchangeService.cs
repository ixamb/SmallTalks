using System;
using System.Collections.Generic;
using SmallTalks.Extensions;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Swipe;
using TheForge.Services.Delayer;
using Random = UnityEngine.Random;

namespace SmallTalks.Services.ChatExchange
{
    public interface IChatExchangeService
    {
        void SendMessage(Guid narrativeId);
        void ExpectSenderAnswer(Guid narrativeId);

        void RegisterNewNarrativeObserver(INewNarrativeObserver observer);
        void UnregisterNewNarrativeObserver(INewNarrativeObserver observer);
        
        void RegisterNewChatMessageObserver(INewChatMessageObserver observer);
        void UnregisterNewChatMessageObserver(INewChatMessageObserver observer);
    }
    
    public sealed class ChatExchangeService : IChatExchangeService, INarrativeRegistrationObserver
    {
        private readonly ILocalSaveService _localSaveService;
        private readonly IDelayerService _delayerService;
        
        private readonly List<INewNarrativeObserver> _newNarrativeObservers = new();
        private readonly List<INewChatMessageObserver> _newChatMessageObservers = new();

        public ChatExchangeService(ILocalSaveService localSaveService, IDelayerService delayerService, INarrativeStackManager narrativeStackManager)
        {
            _localSaveService = localSaveService;
            _delayerService = delayerService;
            
            narrativeStackManager.RegisterNarrativeRegistrationObserver(this);
        }
        
        public void SendMessage(Guid narrativeId)
        {
            _localSaveService.IncreaseNarrativeProgressStep(narrativeId, 1);
            ExpectSenderAnswer(narrativeId);
        }
        
        public void ExpectSenderAnswer(Guid narrativeId)
        {
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
            _newChatMessageObservers.ForEach(o => o.OnNewChatMessageReceived(narrativeId, progressStep!.Value));
        }
        
        // observer function implementation
        public void OnNewNarrativeAccepted(Guid narrativeId)
        {
            _localSaveService.IncreaseNarrativeProgressStep(narrativeId, -1, autoSave: false);
            WaitForAnswer(onWait: () =>
            {
                ReceiveAnswer(narrativeId);
            });
            _newNarrativeObservers.ForEach(h => h.OnNewNarrativeActivated(narrativeId));
        }

        public void RegisterNewNarrativeObserver(INewNarrativeObserver observer) => _newNarrativeObservers.AddUnique(observer);
        public void UnregisterNewNarrativeObserver(INewNarrativeObserver observer) => _newNarrativeObservers.RemoveUnique(observer);
        
        public void RegisterNewChatMessageObserver(INewChatMessageObserver observer) => _newChatMessageObservers.AddUnique(observer);
        public void UnregisterNewChatMessageObserver(INewChatMessageObserver observer) => _newChatMessageObservers.RemoveUnique(observer);
    }
}