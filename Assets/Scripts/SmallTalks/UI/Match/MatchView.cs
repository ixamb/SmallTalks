using System;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Chat;
using TheForge.Extensions;
using TheForge.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace SmallTalks.UI.Match
{
    public sealed class MatchView : View
    {
        [SerializeField] private Image matchProfilePicture;
        [SerializeField] private TMP_Text matchName;
        [SerializeField] private Button navigateToChatButton;
        [SerializeField] private Button closeButton;
        
        private ILocalSaveService _localSaveService;
        private IGameDataService _gameDataService;
        
        [Inject]
        private void Construct(ILocalSaveService localSaveService, IGameDataService gameDataService)
        {
            _localSaveService = localSaveService;
            _gameDataService = gameDataService;
        }
        
        public void Initialize(Guid narrativeId, Sprite profileSprite, string senderName)
        {
            matchProfilePicture.sprite = profileSprite;
            matchName.text = senderName;
            navigateToChatButton.onClick.ReplaceListeners(() =>
            {
                var chatView = ViewService.GetView<ChatView>("chat-view");
                if (chatView)
                {
                    chatView.Initialize(
                        narrativeData: _gameDataService.GetNarrativeData(narrativeId),
                        progressStep: _localSaveService.GetNarrativeProgressStep(narrativeId));
                    chatView.ShowView();
                    HideView();
                }
            });
            
            closeButton.onClick.ReplaceListeners(HideView);
        }
    }
}