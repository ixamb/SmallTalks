using System;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Chat;
using TheForge.Extensions;
using TheForge.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Match
{
    public sealed class MatchView : View
    {
        [SerializeField] private Image matchProfilePicture;
        [SerializeField] private TMP_Text matchName;
        [SerializeField] private Button navigateToChatButton;
        [SerializeField] private Button closeButton;
        
        public void Initialize(Guid narrativeId, Sprite profileSprite, string senderName)
        {
            matchProfilePicture.sprite = profileSprite;
            matchName.text = senderName;
            
            navigateToChatButton.onClick.ReplaceListeners(() =>
            {
                var chatView = ViewService.Instance.GetView<ChatView>("chat-view");
                chatView.Initialize(
                    senderData: (profileSprite, senderName),
                    narrativeId: narrativeId,
                    narrativeEntries: GameDataService.Instance.GetNarrativeDataDictionary()[narrativeId].NarrativeEntries,
                    progressStep: LocalSaveService.Instance.GetNarrativeProgressStep(narrativeId));
                chatView.ShowView();
                HideView();
            });
            
            closeButton.onClick.ReplaceListeners(HideView);
        }
    }
}