using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.Controllers.InGameNotifications
{
    public sealed class InGameNotification : MonoBehaviour
    {
        [SerializeField] private Image notificationImage;
        [SerializeField] private TMP_Text notificationTitle;
        [SerializeField] private TMP_Text notificationMessage;
        [SerializeField] private Button notificationClick;
        [Space]
        [SerializeField] private Animator notificationAnimator;

        public Action OnClick;
        
        private static readonly int Appear = Animator.StringToHash("Appear");
        private static readonly int Disappear = Animator.StringToHash("Disappear");

        public void Initialize(Sprite sprite, string title, string message)
        {
            notificationImage.sprite = sprite;
            notificationTitle.text = title;
            notificationMessage.text = message;
        }
        
        public void Show()
        {
            notificationAnimator.SetTrigger(Appear);
        }

        public void Hide()
        {
            notificationAnimator.SetTrigger(Disappear);
        }
    }
}