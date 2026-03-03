using System;
using System.Collections.Generic;
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
        [SerializeField] private GameObject unreadIndicator;
        [Space]
        [SerializeField] private Transform tagContent;
        [SerializeField] private TagComponent tagComponentPrefab;

        public void Initialize(NarrativePreviewData narrativePreviewData)
        {
            profilePicture.sprite = narrativePreviewData.ProfilePicture;
            name.text = narrativePreviewData.Name;
            description.text = narrativePreviewData.Description;
            narrativePreviewData.Tags.ForEach(textTag => Instantiate(tagComponentPrefab, tagContent).Initialize(textTag));
            clickable.onClick.ReplaceListeners(() => narrativePreviewData.OnClick());
            unreadIndicator.SetActive(narrativePreviewData.Unread);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(tagContent.GetComponent<RectTransform>());
        }
        
        public void UpdateDescription(string newDescription) => description.text = newDescription;
        public void UpdateOnClick(Action newOnClick) => clickable.onClick.ReplaceListeners(() => newOnClick());
        public void UpdateUnreadIndicator(bool newUnread) => unreadIndicator.SetActive(newUnread);

        public sealed record NarrativePreviewData
        {
            public Sprite ProfilePicture { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<string> Tags { get; set; }
            public bool Unread { get; set; }
            public Action OnClick { get; set; }
        }
    }
}
