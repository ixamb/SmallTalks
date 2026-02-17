using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Shared
{
    public sealed class FloatingFooterBarComponent : MonoBehaviour
    {
        [SerializeField] private Button swipeButton;
        [SerializeField] private Button narrativeListButton;
        [SerializeField] private Button settingsButton;

        private const string NarrativeListViewCode = "narrative-list-view";
        private const string SwipeViewCode = "swipe-view";
        private const string SettingsViewCode = "settings-view";

        private void Awake()
        {
            swipeButton.onClick.ReplaceListeners(DisplaySwipeView);
            narrativeListButton.onClick.ReplaceListeners(DisplayNarrativeListView);
            settingsButton.onClick.ReplaceListeners(DisplaySettingsView);
        }

        private void DisplayNarrativeListView()
        {
            TryShowView(NarrativeListViewCode);
            TryHideView(SwipeViewCode);
            TryHideView(SettingsViewCode);
        }
        
        private void DisplaySwipeView()
        {
            TryHideView(NarrativeListViewCode);
            TryShowView(SwipeViewCode);
            TryHideView(SettingsViewCode);
        }
        
        private void DisplaySettingsView()
        {
            TryHideView(NarrativeListViewCode);
            TryHideView(SwipeViewCode);
            TryShowView(SettingsViewCode);
        }

        private static void TryShowView(string viewCode)
        {
            var view = ViewService.Instance.GetView(viewCode);
            if (view is not null && !view.IsVisibleAndActive()!)
                view.ShowView();
        }
        
        private static void TryHideView(string viewCode)
        {
            var view = ViewService.Instance.GetView(viewCode);
            if (view is not null && view.IsVisibleAndActive())
                view.HideView();
        }
    }
}