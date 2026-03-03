using System;
using System.Collections.Generic;
using System.Linq;
using SmallTalks.Data;
using SmallTalks.UI.Chat;
using SmallTalks.UI.NarrativeList.Components;
using TheForge.Services.Views;
using UnityEngine;
using VContainer;

namespace SmallTalks.UI.NarrativeList
{
    public sealed class NarrativeListView : View
    {
        [SerializeField] private Transform chatPreviewContent;
        [SerializeField] private NarrativePreviewComponent narrativePreviewComponentPrefab;

        private INarrativePreviewListPresenter _narrativePreviewListPresenter;
        
        private readonly Dictionary<Guid, NarrativePreviewComponent> _chatPreviewComponents = new();

        [Inject]
        private void Construct(INarrativePreviewListPresenter narrativePreviewListPresenter)
        {
            _narrativePreviewListPresenter = narrativePreviewListPresenter;

            _narrativePreviewListPresenter.OnNewNarrativeAdded += InstantiateNewNarrativeEntry;
            _narrativePreviewListPresenter.OnChatMessageReceived += OnNewChatReceivedHandler;
            _narrativePreviewListPresenter.OnChatMessageRead += MarkChatEntryAsRead;
        }
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var narrativePreview in _narrativePreviewListPresenter.GetActiveNarrativePreviews())
            {
                var spawnedPreview = Instantiate(narrativePreviewComponentPrefab, chatPreviewContent);
                spawnedPreview.Initialize(new NarrativePreviewComponent.NarrativePreviewData
                {
                    ProfilePicture = narrativePreview.Data.Sender.ProfilePicture,
                    Name = narrativePreview.Data.Sender.Name,
                    Description = narrativePreview.LastMessage(),
                    Tags = narrativePreview.Data.Tags.Select(tag => tag.Text).ToList(),
                    Unread = narrativePreview.HasNewMessage,
                    OnClick = () =>
                    {
                        ShowChatView(narrativePreview.Data, narrativePreview.ProgressStep);
                        _narrativePreviewListPresenter.MarkMessageAsRead(narrativePreview.Data.Guid);
                    }
                });
                _chatPreviewComponents.TryAdd(narrativePreview.Data.Guid, spawnedPreview);
            }
        }

        private void InstantiateNewNarrativeEntry(NarrativePreviewListPresenter.ActiveNarrativeEntryPreview newNarrativePreview)
        {
            var narrativePreview = Instantiate(narrativePreviewComponentPrefab, chatPreviewContent);
            narrativePreview.Initialize(new NarrativePreviewComponent.NarrativePreviewData
            {
                ProfilePicture = newNarrativePreview.Data.Sender.ProfilePicture,
                Name = newNarrativePreview.Data.Sender.Name,
                Description = "Nouvelle discussion!",
                Tags = newNarrativePreview.Data.Tags.Select(tag => tag.Text).ToList(),
                Unread = false,
            });
            narrativePreview.transform.SetSiblingIndex(1);
            _chatPreviewComponents.Add(newNarrativePreview.Data.Guid, narrativePreview);
        }

        private void MarkChatEntryAsRead(Guid narrativeId)
        {
            //_chatPreviewComponents[narrativeId]?. // TODO: do something here!
        }

        private void OnNewChatReceivedHandler(NarrativePreviewListPresenter.ActiveNarrativeEntryPreview updatedNarrativePreview, string message)
        {
            if (_chatPreviewComponents.TryGetValue(updatedNarrativePreview.Data.Guid, out var spawnedNarrativePreview))
            {
                spawnedNarrativePreview.UpdateDescription(message);
                spawnedNarrativePreview.UpdateOnClick(() =>
                {
                    ShowChatView(updatedNarrativePreview.Data, updatedNarrativePreview.ProgressStep);
                    _narrativePreviewListPresenter.MarkMessageAsRead(updatedNarrativePreview.Data.Guid);
                });
            }
        }
        
        private void ShowChatView(NarrativeData narrative, int progress)
        {
            var chatView = ViewService.GetView<ChatView>("chat-view");
            if (chatView)
            {
                chatView.Initialize(narrative, progress);
                chatView.ShowView();
            }
        }
    }
}