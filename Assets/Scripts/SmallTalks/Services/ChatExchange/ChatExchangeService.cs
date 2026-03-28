using System;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Swipe;
using TheForge.Services.Delayer;

namespace SmallTalks.Services.ChatExchange
{
    public interface IChatExchangeService
    {
        void SendMessage(Guid narrativeId);
        void ExpectSenderAnswer(Guid narrativeId);

        event Action<ChatExchangeService.NewNarrativeActivatedDto> OnNewNarrativeActivated;
        event Action<ChatExchangeService.NewChatMessageReceivedDto> OnNewChatMessageReceived;
    }
    
    public sealed class ChatExchangeService : IChatExchangeService
    {
        private readonly ILocalSaveService _localSaveService;
        private readonly IDelayerService _delayerService;
        
        public event Action<NewNarrativeActivatedDto> OnNewNarrativeActivated = delegate { };
        public event Action<NewChatMessageReceivedDto> OnNewChatMessageReceived =  delegate { };
        
        public ChatExchangeService(ILocalSaveService localSaveService, IDelayerService delayerService, INarrativeStackManager narrativeStackManager)
        {
            _localSaveService = localSaveService;
            _delayerService = delayerService;

            narrativeStackManager.OnNewNarrativeAccepted += OnNewNarrativeAccepted;
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
            var waitInSeconds = UnityEngine.Random.Range(2, 5f);
            _delayerService.Delay(waitInSeconds, onWait);
        }

        private void ReceiveAnswer(Guid narrativeId)
        {
            _localSaveService.IncreaseNarrativeProgressStep(narrativeId, 1, autoSave: false);
            if (!_localSaveService.TryGetNarrativeProgressStep(narrativeId, out var progressStep))
                return;
            
            _localSaveService.MarkNarrativeProgressNewMessageStatus(narrativeId, true, autoSave: false);
            _localSaveService.Save();
            OnNewChatMessageReceived(new NewChatMessageReceivedDto(narrativeId, progressStep!.Value));
        }
        
        private void OnNewNarrativeAccepted(Guid narrativeId)
        {
            _localSaveService.IncreaseNarrativeProgressStep(narrativeId, -1, autoSave: false);
            WaitForAnswer(onWait: () =>
            {
                ReceiveAnswer(narrativeId);
            });
            OnNewNarrativeActivated(new NewNarrativeActivatedDto(narrativeId));
        }

        public sealed record NewNarrativeActivatedDto (Guid NarrativeId);
        public sealed record NewChatMessageReceivedDto (Guid NarrativeId, int ProgressStep);
        public sealed record NewChatReceivedWithMetadataDto (Guid NarrativeId, UnityEngine.Sprite ProfilePicture, string Name, string Message);
    }
}