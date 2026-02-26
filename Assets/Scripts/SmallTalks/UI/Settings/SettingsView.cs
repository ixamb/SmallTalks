using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Popup;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Settings
{
    public sealed class SettingsView : View
    {
        [SerializeField] private Button deleteDataButton;

        private void Start()
        {
            deleteDataButton.onClick.ReplaceListeners(RequestDeleteNarrativeConfirm);
        }

        private void RequestDeleteNarrativeConfirm()
        {
            var popupConfirmationConfiguration = new PopupView.PopupConfiguration
            {
                MainText = "Êtes-vous certain de vouloir supprimer votre progression ? Cette action est irréversible.",
                ShowCancelButton = true,
                ValidateButtonText = "Supprimer",
                ValidateAction = () =>
                {
                    LocalSaveService.Instance.DeleteAllNarrativeProgress();
                }
            };
            
            var popupView = ViewService.Instance.GetView<PopupView>("popup-view");
            popupView.Initialize(popupConfirmationConfiguration);
            popupView.ShowView();
        }
    }
}