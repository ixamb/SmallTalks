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
using VContainer;

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

        private IGameDataService _gameDataService;
        private ILocalSaveService _localSaveService;
        private IChatExchangeService _chatExchangeService;
        private IDelayerService _delayerService;
        
        private Dictionary<Guid,NarrativeData> _pendingNarratives = new();
        private Guid _topNarrativeId;

        [Inject]
        private void Construct(IGameDataService gameDataService, ILocalSaveService localSaveService,
            IChatExchangeService chatExchangeService, IDelayerService delayerService)
        {
            _gameDataService = gameDataService;
            _localSaveService = localSaveService;
            _chatExchangeService = chatExchangeService;
            _delayerService = delayerService;
        }
        
        protected override void Awake()
        {
            base.Awake();
            likeButton.onClick.ReplaceListeners(OnLikeButtonClicked);
            dislikeButton.onClick.ReplaceListeners(OnDislikeButtonClicked);
        }

        private void Start()
        {
            _pendingNarratives = _gameDataService.GetNarrativeDataDictionary();
            foreach (var narrativeProgressStep in _localSaveService.GetAllNarrativeProgressSteps())
            {
                _pendingNarratives.Remove(narrativeProgressStep.Key);
            }
            cardComponent.OnSwapAnimation = InitializeNextNarrativeOnStack;
            InitializeNextNarrativeOnStack();
        }

        private void OnLikeButtonClicked()
        {
            _localSaveService.RegisterNewNarrativeProgress(_topNarrativeId, true);
            _chatExchangeService.ExpectSenderAnswer(_topNarrativeId, isFirstMessage: true);
            
            ViewService.GetView<NarrativeListView>("narrative-list-view")?.OnNewChatReceivedHandler(_topNarrativeId, -1);
            ShowMatchView();
            
            _pendingNarratives.Remove(_topNarrativeId);
            cardComponent.LikeSwapAnimation();
        }

        private void ShowMatchView()
        {
            var narrativeId = _topNarrativeId;
            var narrative = _pendingNarratives[narrativeId];
            _delayerService.Delay(.5f, () =>
            {
                var matchView = ViewService.GetView<MatchView>("match-view");
                if (matchView)
                {
                    matchView.Initialize(narrativeId, narrative.Sender.ProfilePicture, narrative.Sender.Name);
                    matchView.ShowView();
                }
            });
        }

        private void OnDislikeButtonClicked()
        {
            _localSaveService.RegisterNewNarrativeProgress(_topNarrativeId, false);
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