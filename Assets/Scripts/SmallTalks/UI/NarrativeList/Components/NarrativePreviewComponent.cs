using System;
using SmallTalks.UI.Shared;
using TheForge.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.NarrativeList.Components
{
    public sealed class NarrativePreviewComponent : MonoBehaviour
    {
        [SerializeField] private Image profilePicture;
        [SerializeField] private new TMP_Text name;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Button clickable;
        [Space]
        [SerializeField] private Transform tagContent;
        [SerializeField] private TagComponent tagComponentPrefab;

        public void Initialize(NarrativePreviewData narrativePreviewData)
        {
            profilePicture.sprite = narrativePreviewData.ProfilePicture;
            name.text = narrativePreviewData.Name;
            description.text = narrativePreviewData.Description;
            clickable.onClick.ReplaceListeners(() => narrativePreviewData.OnClick());
        }

        public sealed record NarrativePreviewData
        {
            public Sprite ProfilePicture { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Action OnClick { get; set; }
        }
    }
}
