using SmallTalks.Services.LocalSave;
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
            deleteDataButton.onClick.ReplaceListeners(() => LocalSaveService.Instance.DeleteAllNarrativeProgress());
        }
    }
}