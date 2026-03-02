using SmallTalks.Services.LocalSave;
using SmallTalks.UI.Popup;
using TheForge.Extensions;
using TheForge.Services.Scenes;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace SmallTalks.UI.Settings
{
    public sealed class SettingsView : View
    {
        [SerializeField] private Button aboutButton;
        [SerializeField] private Button deleteDataButton;

        private ISceneService _sceneService;
        private ILocalSaveService _localSaveService;
        
        [Inject]
        private void Construct(ISceneService sceneService, ILocalSaveService localSaveService)
        {
            _sceneService = sceneService;
            _localSaveService = localSaveService;
        }
        
        private void Start()
        {
            aboutButton.onClick.ReplaceListeners(InitializeAboutPopup);
            deleteDataButton.onClick.ReplaceListeners(RequestDeleteNarrativeConfirm);
        }

        private void InitializeAboutPopup()
        {
            var aboutPopup = new PopupView.PopupConfiguration
            {
                MainText =
                    $"Small Talks\n" +
                    $"Version {Application.version}",
                ValidateButtonText = "OK"
            };

            var popupView = ViewService.GetView<PopupView>("popup-view");
            if (popupView)
            {
                popupView.Initialize(aboutPopup);
                popupView.ShowView();
            }
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
                    _localSaveService.DeleteAllNarrativeProgress();
                    ViewService.GetView("popup-view")?.HideView();
                    _sceneService.LoadSceneAsync(Constants.SceneNames.Intro, LoadSceneMode.Single);
                }
            };
            
            var popupView = ViewService.GetView<PopupView>("popup-view");
            if (popupView)
            {
                popupView.Initialize(popupConfirmationConfiguration);
                popupView.ShowView();
            }
        }
    }
}