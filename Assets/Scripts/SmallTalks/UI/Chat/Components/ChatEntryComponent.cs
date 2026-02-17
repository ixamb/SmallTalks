using TMPro;
using UnityEngine;

namespace SmallTalks.UI.Chat.Components
{
    public sealed class ChatEntryComponent :  MonoBehaviour
    {
        [SerializeField] private TMP_Text chatEntryText;

        public void Initialize(string text)
        {
            chatEntryText.text = text;
        }
    }
}