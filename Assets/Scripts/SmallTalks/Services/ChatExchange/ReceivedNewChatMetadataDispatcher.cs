using System;
using System.Collections.Generic;
using SmallTalks.Extensions;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.GameData;
using UnityEngine;
using VContainer;

namespace SmallTalks.Services.ChatExchange
{
    public sealed class ReceivedNewChatMetadataDispatcher : MonoBehaviour, INewChatMessageObserver
    {
        private IGameDataService _gameDataService;
        
        private static readonly List<IChatReceivedHandlerWithMetadata> HandlersWithMetadata = new();

        [Inject]
        private void Construct(IChatExchangeService chatExchangeService, IGameDataService gameDataService)
        {
            _gameDataService = gameDataService;
            chatExchangeService.RegisterNewChatMessageObserver(this);
        }
        
        // observer function
        public void OnNewChatMessageReceived(Guid narrativeId, int progressStep)
        {
            var narrative = _gameDataService.GetNarrativeData(narrativeId);
            if (narrative is null)
                return;
            HandlersWithMetadata.ForEach(h => h.OnNewChatReceivedHandler(
                narrativeId: narrativeId,
                profilePicture: narrative.Sender.ProfilePicture,
                name: narrative.Sender.Name,
                message: narrative.NarrativeEntries[progressStep].Entry));
        }

        public static void RegisterObserver(IChatReceivedHandlerWithMetadata observer) => HandlersWithMetadata.AddUnique(observer);
        public static void UnregisterObserver(IChatReceivedHandlerWithMetadata observer) => HandlersWithMetadata.Remove(observer);
    }
}