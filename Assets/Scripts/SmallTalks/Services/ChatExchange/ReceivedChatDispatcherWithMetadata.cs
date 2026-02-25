using System;
using System.Collections.Generic;
using SmallTalks.Extensions;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.GameData;
using UnityEngine;

namespace SmallTalks.Services.ChatExchange
{
    public sealed class ReceivedChatDispatcherWithMetadata : MonoBehaviour, IChatReceivedHandler
    {
        private static readonly List<IChatReceivedHandlerWithMetadata> HandlersWithMetadata = new();

        private void Start()
        {
            ChatExchangeService.Instance.RegisterChatReceivedHandler(this);
        }

        public void OnNewChatReceivedHandler(Guid narrativeId, int progressStep)
        {
            var narrative = GameDataService.Instance.GetNarrativeData(narrativeId);
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