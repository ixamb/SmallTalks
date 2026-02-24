using System;
using System.Collections.Generic;
using System.Linq;
using SmallTalks.Data;
using SmallTalks.Extensions;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Chat;
using SmallTalks.UI.NarrativeList.Components;
using TheForge.Services.Views;
using UnityEngine;

namespace SmallTalks.UI.NarrativeList
{
    public sealed class NarrativeListView : View, IChatReceivedHandler
    {
        [SerializeField] private Transform chatPreviewContent;
        [SerializeField] private NarrativePreviewComponent narrativePreviewComponentPrefab;

        private readonly Dictionary<Guid, NarrativePreviewComponent> _chatPreviewComponents = new();
        
        private void Start()
        {
            Initialize();
            ChatExchangeService.Instance.RegisterChatReceivedHandler(this);
        }

        // purpose of initial listing display on screen
        private void Initialize()
        {
            var narrativeData = GameDataService.Instance.GetNarrativeData();

            foreach (var runningNarrativeKvp in LocalSaveService.Instance.GetAllNarrativeProgressSteps()
                         .Where(runningNarrativeKvp => runningNarrativeKvp.Value.Accepted)
                         .OrderBy(runningNarrativeKvp => runningNarrativeKvp.Value.LastUpdate))
            {
                var kvp = runningNarrativeKvp;
                
                if (!narrativeData.TryGetValue(kvp.Key, out var data))
                    continue;
                
                var narrativePreview = Instantiate(narrativePreviewComponentPrefab, chatPreviewContent);
                narrativePreview.Initialize(new NarrativePreviewComponent.NarrativePreviewData
                {
                    ProfilePicture = data.Sender.ProfilePicture,
                    Name = data.Sender.Name,
                    Description = data.NarrativeEntries.IsIndexValid(runningNarrativeKvp.Value.Progress) ? data.NarrativeEntries[runningNarrativeKvp.Value.Progress].Entry : "Nouvelle discussion!",
                    Unread = kvp.Value.HasNewMessage,
                    OnClick = () =>
                    {
                        if (!data.NarrativeEntries.IsIndexValid(runningNarrativeKvp.Value.Progress))
                            return;
                        
                        ShowChatView(data, kvp.Value.Progress);
                        if (kvp.Value.HasNewMessage)
                            LocalSaveService.Instance.MarkNarrativeProgressNewMessageStatus(kvp.Key, false);
                    }
                });
                _chatPreviewComponents.TryAdd(kvp.Key, narrativePreview);
            }
        }

        public void OnNewChatReceivedHandler(Guid narrativeId, int progressStep)
        {
            var narratives = GameDataService.Instance.GetNarrativeData();
            var narrativeData = narratives[narrativeId];
            if (narrativeData is null)
                return;
            
            if (_chatPreviewComponents.TryGetValue(narrativeId, out var spawnedNarrativePreview))
            {
                spawnedNarrativePreview.UpdateDescription(narrativeData.NarrativeEntries[progressStep].Entry);
                spawnedNarrativePreview.UpdateOnClick(() => ShowChatView(narrativeData, progressStep));
            }
            else
            {
                var narrativePreview = Instantiate(narrativePreviewComponentPrefab, chatPreviewContent);
                narrativePreview.Initialize(new NarrativePreviewComponent.NarrativePreviewData
                {
                    ProfilePicture = narrativeData.Sender.ProfilePicture,
                    Name = narrativeData.Sender.Name,
                    Description = narrativeData.NarrativeEntries.IsIndexValid(progressStep) ? narrativeData.NarrativeEntries[progressStep].Entry : "Nouvelle discussion!",
                    Unread = false,
                    OnClick = () =>
                    {
                        if (!narrativeData.NarrativeEntries.IsIndexValid(progressStep))
                            return;
                        
                        ShowChatView(narrativeData, progressStep);
                    }
                });
                narrativePreview.transform.SetSiblingIndex(1);
                _chatPreviewComponents.Add(narrativeId, narrativePreview);
            }
        }
        
        private static void ShowChatView(NarrativeData narrative, int progress)
        {
            var chatView = ViewService.Instance.GetView<ChatView>("chat-view");
            chatView.Initialize((narrative.Sender.ProfilePicture, narrative.Sender.Name), narrative.Guid, narrative.NarrativeEntries, progress);
            chatView.ShowView();
        }
    }
}