using System;
using TheForge.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Chat.Components
{
    [RequireComponent(typeof(Button))]
    public sealed class KeyboardKeyComponent : MonoBehaviour
    {
        [SerializeField] private KeyType keyType = KeyType.Typing;
        
        public void InitializeInputAction(Action onInput)
        {
            GetComponent<Button>().onClick.ReplaceListeners(() => onInput());
        }
        
        public KeyType GetKeyType() => keyType;

        public enum KeyType
        {
            None, Typing, KeyboardDown,
        }
    }
}