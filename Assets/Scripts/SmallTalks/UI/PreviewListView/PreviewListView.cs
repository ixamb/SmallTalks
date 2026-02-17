using System.Collections.Generic;
using SmallTalks.UI.ChatListView.Components;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;

namespace SmallTalks.UI.PreviewListView
{
    public sealed class PreviewListView : View
    {
        [SerializeField] private Transform chatPreviewContent;
        [SerializeField] private ChatPreviewComponent chatPreviewComponentPrefab;

        private readonly List<ChatPreviewComponent> _chatPreviewComponents = new();
        
        private void Start()
        {
            OnShow += Initialize;
        }

        public void Initialize()
        {
            _chatPreviewComponents.DestroyAndClear();
        }
    }
}