using System;
using TheForge.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Chat.Components
{
    [RequireComponent(typeof(Button))]
    public class KeyboardKeyComponent : MonoBehaviour
    {
        public void InitializeInputAction(Action onInput)
        {
            GetComponent<Button>().onClick.ReplaceListeners(() => onInput());
        }
    }
}