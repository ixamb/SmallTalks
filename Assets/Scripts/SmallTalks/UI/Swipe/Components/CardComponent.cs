using System;
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
        [Space]
        [SerializeField] private Animator cardAnimator;
        
        public Action OnSwapAnimation;
        
        private static readonly int Property = Animator.StringToHash("Swap Like");
        private static readonly int Property1 = Animator.StringToHash("Swap Dislike");
        
        public void Initialize(Sprite sprite, string name, string description)
        {
            topCardImage.sprite = sprite;
            topCardName.text = name;
            topCardDescription.text = description;
        }
        
        public void LikeSwapAnimation() => cardAnimator.SetTrigger(Property);
        public void DislikeSwapAnimation() => cardAnimator.SetTrigger(Property1);
        
        public void OnSwap() => OnSwapAnimation?.Invoke();
    }
}