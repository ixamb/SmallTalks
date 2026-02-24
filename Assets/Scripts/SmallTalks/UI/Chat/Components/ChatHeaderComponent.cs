using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Chat.Components
{
    public sealed class ChatHeaderComponent : MonoBehaviour
    {
        [SerializeField] private Image headerImage;
        [SerializeField] private TMP_Text headerText;

        public void Initialize(Sprite sprite, string text)
        {
            headerImage.sprite = sprite;
            headerText.text = text;
        }
    }
}