using System;
using System.Collections.Generic;
using System.Linq;
using SmallTalks.Data;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Match;
using SmallTalks.UI.NarrativeList;
using SmallTalks.UI.Swipe.Components;
using TheForge.Extensions;
using TheForge.Services.Delayer;
using TheForge.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Swipe
{
    public sealed class SwipeView : View
    {
        [Header("Top card info")]
        [SerializeField] private CardComponent cardComponent;
        [SerializeField] private TMP_Text emptyDescription;
        [Space]
        [Header("Interaction fields")]
        [SerializeField] private Button likeButton;
        [SerializeField] private Button dislikeButton;
        
        private Dictionary<Guid,NarrativeData> _pendingNarratives = new();
        private Guid _topNarrativeId;

        protected override void Awake()
        {
            base.Awake();
            likeButton.onClick.ReplaceListeners(OnLikeButtonClicked);
            dislikeButton.onClick.ReplaceListeners(OnDislikeButtonClicked);
        }

        private void Start()
        {
            _pendingNarratives = GameDataService.Instance.GetNarrativeDataDictionary();
            foreach (var narrativeProgressStep in LocalSaveService.Instance.GetAllNarrativeProgressSteps())
            {
                _pendingNarratives.Remove(narrativeProgressStep.Key);
            }
            cardComponent.OnSwapAnimation = InitializeNextNarrativeOnStack;
            InitializeNextNarrativeOnStack();
        }

        private void OnLikeButtonClicked()
        {
            LocalSaveService.Instance.RegisterNewNarrativeProgress(_topNarrativeId, true);
            ChatExchangeService.Instance.ExpectSenderAnswer(_topNarrativeId, isFirstMessage: true);
            
            ViewService.Instance.GetView<NarrativeListView>("narrative-list-view").OnNewChatReceivedHandler(_topNarrativeId, -1);
            ShowMatchView();
            
            _pendingNarratives.Remove(_topNarrativeId);
            cardComponent.LikeSwapAnimation();
        }

        private void ShowMatchView()
        {
            var narrativeId = _topNarrativeId;
            var narrative = _pendingNarratives[narrativeId];
            ActionDelayerService.Instance.Delay(.5f, () =>
            {
                var matchView = ViewService.Instance.GetView<MatchView>("match-view");
                matchView.Initialize(narrativeId, narrative.Sender.ProfilePicture, narrative.Sender.Name);
                matchView.ShowView();
            });
        }

        private void OnDislikeButtonClicked()
        {
            LocalSaveService.Instance.RegisterNewNarrativeProgress(_topNarrativeId, false);
            _pendingNarratives.Remove(_topNarrativeId);
            cardComponent.DislikeSwapAnimation();
        }

        private void InitializeNextNarrativeOnStack()
        {
            var remainingPendingNarrative = _pendingNarratives.Any();
            cardComponent.gameObject.SetActive(remainingPendingNarrative);
            emptyDescription.gameObject.SetActive(!remainingPendingNarrative);

            if (!remainingPendingNarrative)
            {
                likeButton.interactable = false;
                dislikeButton.interactable = false;
                return;
            }

            var topNarrative = _pendingNarratives.First();
            cardComponent.Initialize(topNarrative.Value.Sender.ProfilePicture, topNarrative.Value.Sender.Name, topNarrative.Value.Sender.Description);
            _topNarrativeId = topNarrative.Key;
        }
    }
}