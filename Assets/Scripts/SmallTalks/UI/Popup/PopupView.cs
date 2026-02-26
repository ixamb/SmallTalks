#nullable enable
using System;
using TheForge.Extensions;
using TheForge.Services.Views;
using TheForge.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Popup
{
    public sealed class PopupView : View
    {
        [SerializeField] private TMP_Text mainText;
        [SerializeField] private LabeledButton validateButton;
        [SerializeField] private Button cancelButton;

        private void Start()
        {
            cancelButton.onClick.ReplaceListeners(HideView);
        }

        public void Initialize(PopupConfiguration configuration)
        {
            mainText.text = configuration.MainText;
            validateButton.Initialize(configuration.ValidateButtonText, () =>
            {
                configuration.ValidateAction?.Invoke();
                HideView();
            });
            cancelButton.gameObject.SetActive(configuration.ShowCancelButton);
        }

        public sealed record PopupConfiguration
        {
            public string MainText { get; set; } = string.Empty;
            public string ValidateButtonText { get; set; } = string.Empty;
            public Action? ValidateAction { get; set; }
            public bool ShowCancelButton { get; set; }
        }
    }
}