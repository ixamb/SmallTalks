using System;
using System.Collections.Generic;
using System.Linq;
using SmallTalks.Data;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Swipe.Components;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Swipe
{
    public sealed class SwipeView : View
    {
        [Header("Top card info")]
        [SerializeField] private CardComponent cardComponent;
        [Space]
        [Header("Interaction fields")]
        [SerializeField] private Button likeButton;
        [SerializeField] private Button dislikeButton;
        
        private Dictionary<Guid,NarrativeData> _pendingNarratives = new();
        private Guid _topNarrative;

        protected override void Awake()
        {
            base.Awake();
            likeButton.onClick.ReplaceListeners(OnLikeButtonClicked);
            dislikeButton.onClick.ReplaceListeners(OnDislikeButtonClicked);
        }

        private void Start()
        {
            _pendingNarratives = GameDataService.Instance.NarrativeData();
            foreach (var narrativeProgressStep in LocalSaveService.Instance.GetAllNarrativeProgressSteps())
            {
                _pendingNarratives.Remove(narrativeProgressStep.Key);
            }
            InitializeNextNarrativeOnStack();
        }

        private void OnLikeButtonClicked()
        {
            LocalSaveService.Instance.RegisterNarrativeProgress(_topNarrative, true);
            _pendingNarratives.Remove(_topNarrative);
            InitializeNextNarrativeOnStack();
        }

        private void OnDislikeButtonClicked()
        {
            LocalSaveService.Instance.RegisterNarrativeProgress(_topNarrative, false);
            _pendingNarratives.Remove(_topNarrative);
            InitializeNextNarrativeOnStack();
        }

        private void InitializeNextNarrativeOnStack()
        {
            if (_pendingNarratives.Any())
            {
                cardComponent.gameObject.SetActive(false);
                return;
            }
            
            var topNarrative = _pendingNarratives.First();
            cardComponent.Initialize(topNarrative.Value.Sender.ProfilePicture, topNarrative.Value.Sender.Name, topNarrative.Value.Sender.Description);
            _topNarrative = topNarrative.Key;
        }
    }
}