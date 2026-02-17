using SmallTalks.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.ChatListView.Components
{
    public sealed class ChatPreviewComponent : MonoBehaviour
    {
        [SerializeField] private Image profilePicture;
        [SerializeField] private new TMP_Text name;
        [SerializeField] private TMP_Text description;
        [Space]
        [SerializeField] private Transform tagContent;
        [SerializeField] private TagComponent tagComponentPrefab;

        public void Initialize(ChatPreviewData chatPreviewData)
        {
            profilePicture.sprite = chatPreviewData.ProfilePicture;
            name.text = chatPreviewData.Name;
            description.text = chatPreviewData.Description;
        }

        public sealed record ChatPreviewData
        {
            public Sprite ProfilePicture { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
