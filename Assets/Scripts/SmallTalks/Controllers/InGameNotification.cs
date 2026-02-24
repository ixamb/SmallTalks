using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.Services.InGameNotifications
{
    public sealed class InGameNotification : MonoBehaviour
    {
        [SerializeField] private Image notificationImage;
        [SerializeField] private TMP_Text notificationTitle;
        [SerializeField] private TMP_Text notificationMessage;
    }
}