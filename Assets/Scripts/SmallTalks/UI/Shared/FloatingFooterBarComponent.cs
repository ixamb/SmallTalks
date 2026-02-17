using SmallTalks.UI.Tools;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Shared
{
    public sealed class FloatingFooterBarComponent : MonoBehaviour
    {
        [SerializeField] private HorizontalViewNavigation horizontalViewNavigation;
        [Space]
        [SerializeField] private Button swipeButton;
        [SerializeField] private Button narrativeListButton;
        [SerializeField] private Button settingsButton;

        private const string NarrativeListViewCode = "narrative-list-view";
        private const string SwipeViewCode = "swipe-view";
        private const string SettingsViewCode = "settings-view";

        private void Awake()
        {
            swipeButton.onClick.ReplaceListeners(() => NavigateTo(SwipeViewCode));
            narrativeListButton.onClick.ReplaceListeners(() => NavigateTo(NarrativeListViewCode));
            settingsButton.onClick.ReplaceListeners(() => NavigateTo(SettingsViewCode));
        }

        private void NavigateTo(string viewCode)
        {
            horizontalViewNavigation
                .NavigateTo(((View)ViewService.Instance.GetView(viewCode))?.GetComponent<HorizontalViewNavElement>());
        }
    }
}