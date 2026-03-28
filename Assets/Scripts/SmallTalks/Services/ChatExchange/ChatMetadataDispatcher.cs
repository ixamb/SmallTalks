using System;
using SmallTalks.Services.GameData;
using VContainer;

namespace SmallTalks.Services.ChatExchange
{
    public interface IChatMetadataDispatcher
    {
        event Action<ChatExchangeService.NewChatReceivedWithMetadataDto> OnNewChatReceivedWithMetadata;
    }
    
    public sealed class ChatMetadataDispatcher : IChatMetadataDispatcher
    {
        public event Action<ChatExchangeService.NewChatReceivedWithMetadataDto> OnNewChatReceivedWithMetadata = delegate { };
        
        private IGameDataService _gameDataService;
        
        [Inject]
        private void Construct(IChatExchangeService chatExchangeService, IGameDataService gameDataService)
        {
            _gameDataService = gameDataService;
            chatExchangeService.OnNewChatMessageReceived += dto => OnNewChatMessageReceived(dto.NarrativeId, dto.ProgressStep);
        }
        
        public void OnNewChatMessageReceived(Guid narrativeId, int progressStep)
        {
            var narrative = _gameDataService.GetNarrativeData(narrativeId);
            if (narrative is null)
                return;
            
            OnNewChatReceivedWithMetadata.Invoke(new ChatExchangeService.NewChatReceivedWithMetadataDto(narrativeId, narrative.Sender.ProfilePicture, narrative.Sender.Name, narrative.NarrativeEntries[progressStep].Entry));
        }
    }
}