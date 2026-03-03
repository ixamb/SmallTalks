using SmallTalks.Data;
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

        private INarrativeStackManager _narrativeStackManager;
        private IDelayerService _delayerService;

        [Inject]
        private void Construct(INarrativeStackManager narrativeStackManager, IDelayerService delayerService)
        {
            _narrativeStackManager = narrativeStackManager;
            _delayerService = delayerService;
        }
        
        protected override void Awake()
        {
            base.Awake();
            likeButton.onClick.ReplaceListeners(OnLikeButtonClicked);
            dislikeButton.onClick.ReplaceListeners(OnDislikeButtonClicked);
            cardComponent.OnSwapAnimation = InitializeNextNarrativeOnStack;
            InitializeNextNarrativeOnStack();
        }

        private void OnLikeButtonClicked()
        {
            var narrative = _narrativeStackManager.AcceptNarrative();
            ShowMatchView(narrative);
            cardComponent.LikeSwapAnimation();
        }

        private void ShowMatchView(NarrativeData narrativeData)
        {
            _delayerService.Delay(.5f, () =>
            {
                var matchView = ViewService.GetView<MatchView>("match-view");
                if (matchView)
                {
                    matchView.Initialize(narrativeData.Guid, narrativeData.Sender.ProfilePicture, narrativeData.Sender.Name);
                    matchView.ShowView();
                }
            });
        }

        private void OnDislikeButtonClicked()
        {
            _narrativeStackManager.RefuseNarrative();
            cardComponent.DislikeSwapAnimation();
        }

        private void InitializeNextNarrativeOnStack()
        {
            var nextPendingNarrative = _narrativeStackManager.GetNextNarrative();
            cardComponent.gameObject.SetActive(nextPendingNarrative is not null);
            emptyDescription.gameObject.SetActive(nextPendingNarrative is null);

            if (nextPendingNarrative is null)
            {
                likeButton.interactable = false;
                dislikeButton.interactable = false;
                return;
            }

            cardComponent.Initialize(nextPendingNarrative.Sender.ProfilePicture, nextPendingNarrative.Sender.Name, nextPendingNarrative.Sender.Description);
        }
    }
}