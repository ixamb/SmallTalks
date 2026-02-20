using System;
using System.Collections.Generic;
using System.Linq;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Chat;
using SmallTalks.UI.NarrativeList.Components;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;

namespace SmallTalks.UI.NarrativeList
{
    public sealed class NarrativeListView : View
    {
        [SerializeField] private Transform chatPreviewContent;
        [SerializeField] private NarrativePreviewComponent narrativePreviewComponentPrefab;

        private readonly List<NarrativePreviewComponent> _chatPreviewComponents = new();
        
        private void Start()
        {
            OnShow += Initialize;
        }

        private void Initialize()
        {
            _chatPreviewComponents.DestroyAndClear();

            foreach (var runningNarrativeKvp in LocalSaveService.Instance.GetAllNarrativeProgressSteps()
                         .Where(runningNarrativeKvp => runningNarrativeKvp.Value.Accepted))
            {
                if (!GameDataService.Instance.NarrativeData().TryGetValue(runningNarrativeKvp.Key, out var narrativeData))
                    continue;
                
                var narrativePreview = Instantiate(narrativePreviewComponentPrefab, chatPreviewContent);
                narrativePreview.Initialize(new NarrativePreviewComponent.NarrativePreviewData
                {
                    ProfilePicture = narrativeData.Sender.ProfilePicture,
                    Name = narrativeData.Sender.Name,
                    Description = narrativeData.Sender.Description,
                    OnClick = () => ShowChatView(runningNarrativeKvp.Key, runningNarrativeKvp.Value.Progress)
                });
                _chatPreviewComponents.Add(narrativePreview);
            }
        }

        private void ShowChatView(Guid narrativeId, uint progress)
        {
            var narrative = GameDataService.Instance.NarrativeData()[narrativeId];
            
            var chatView = ViewService.Instance.GetView<ChatView>("chat-view");
            chatView.Initialize(narrative.NarrativeEntries, progress);
        }
    }
}