using System;
using JetBrains.Annotations;
using SmallTalks.Data;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.ChatExchange.Observers;

namespace SmallTalks.UI.Chat
{
    public interface IChatPresenter
    {
        void Initialize(NarrativeData narrativeData);
        void ReplaceReceivedChatMessageEvent(Action<Guid, int> onNewChatMessageReceivedEvent);
        void Clear();
        void SendMessageRequest();
        NarrativeData GetNarrativeData();
        bool ChatIsAvailable(int progressStep);
    }
    
    public sealed class ChatPresenter : IChatPresenter, INewChatMessageObserver
    {
        private readonly IChatExchangeService _chatExchangeService;

        private event Action<Guid, int> OnNewChatMessageReceivedEvent;
        
        [CanBeNull] private NarrativeData _narrativeData;
        
        public ChatPresenter(IChatExchangeService chatExchangeService)
        {
            _chatExchangeService = chatExchangeService;
            _chatExchangeService.RegisterNewChatMessageObserver(this);
        }

        public void Initialize(NarrativeData narrativeData)
        {
            _narrativeData = narrativeData;
        }
        
        public void ReplaceReceivedChatMessageEvent(Action<Guid, int> onNewChatMessageReceivedEvent)
            => OnNewChatMessageReceivedEvent = onNewChatMessageReceivedEvent;

        public void Clear()
        {
            _narrativeData = null;
            _chatExchangeService.UnregisterNewChatMessageObserver(this);
            OnNewChatMessageReceivedEvent = null;
        }

        public void SendMessageRequest()
        {
            if (_narrativeData is not null)
                _chatExchangeService.SendMessage(_narrativeData.Guid);
        }

        public bool ChatIsAvailable(int progressStep)
        {
            if (_narrativeData is not null)
                return progressStep + 1 < _narrativeData.NarrativeEntries.Count && _narrativeData.NarrativeEntries[progressStep + 1].Sender == NarrativeData.NarrativeEntry.MessageSender.Myself;
            return false;
        }
        
        public NarrativeData GetNarrativeData() => _narrativeData;

        // observer function
        public void OnNewChatMessageReceived(Guid narrativeId, int progressStep)
        {
            OnNewChatMessageReceivedEvent?.Invoke(narrativeId, progressStep);
        }
    }
}