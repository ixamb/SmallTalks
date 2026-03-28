using System;
using SmallTalks.Services.GameData;
using UnityEngine;
using VContainer;

namespace SmallTalks.Services.ChatExchange
{
    public sealed class ReceivedNewChatMetadataDispatcher : MonoBehaviour
    {
        public static event Action<ChatExchangeService.NewChatReceivedWithMetadataDto> OnNewChatReceivedWithMetadata = delegate { };
        
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