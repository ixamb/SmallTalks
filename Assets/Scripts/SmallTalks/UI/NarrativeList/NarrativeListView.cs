using System.Collections.Generic;
using System.Linq;
using SmallTalks.Data;
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
            Initialize();
        }

        private void Initialize()
        {
            var narrativeData = GameDataService.Instance.NarrativeData();
            _chatPreviewComponents.DestroyAndClear();

            foreach (var runningNarrativeKvp in LocalSaveService.Instance.GetAllNarrativeProgressSteps()
                         .Where(runningNarrativeKvp => runningNarrativeKvp.Value.Accepted))
            {
                var kvp = runningNarrativeKvp;
                
                if (!narrativeData.TryGetValue(kvp.Key, out var data))
                    continue;
                
                var narrativePreview = Instantiate(narrativePreviewComponentPrefab, chatPreviewContent);
                narrativePreview.Initialize(new NarrativePreviewComponent.NarrativePreviewData
                {
                    ProfilePicture = data.Sender.ProfilePicture,
                    Name = data.Sender.Name,
                    Description = data.Sender.Description,
                    OnClick = () => ShowChatView(data, kvp.Value.Progress)
                });
                _chatPreviewComponents.Add(narrativePreview);
            }
        }

        private void ShowChatView(NarrativeData narrative, uint progress)
        {
            
            var chatView = ViewService.Instance.GetView<ChatView>("chat-view");
            chatView.Initialize(narrative.NarrativeEntries, progress);
            chatView.ShowView();
        }
    }
}