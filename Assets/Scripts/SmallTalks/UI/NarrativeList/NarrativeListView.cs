using System.Collections.Generic;
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

        public void Initialize()
        {
            _chatPreviewComponents.DestroyAndClear();
        }
    }
}