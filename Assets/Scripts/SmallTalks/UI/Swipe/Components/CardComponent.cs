using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Swipe.Components
{
    public sealed class CardComponent : MonoBehaviour
    {
        [Header("Top card info")]
        [SerializeField] private Image topCardImage;
        [SerializeField] private TMP_Text topCardName;
        [SerializeField] private TMP_Text topCardDescription;

        public void Initialize(Sprite sprite, string name, string description)
        {
            topCardImage.sprite = sprite;
            topCardName.text = name;
            topCardDescription.text = description;
        }
    }
}