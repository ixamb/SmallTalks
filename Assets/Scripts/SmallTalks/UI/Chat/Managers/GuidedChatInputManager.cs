using System;
using JetBrains.Annotations;
using SmallTalks.UI.Chat.Components;
using TheForge.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Chat.Managers
{
    public sealed class GuidedChatInputManager : MonoBehaviour
    {
        [SerializeField] private GameObject footer;
        [SerializeField] private GuidedChatInputComponent guidedChatInputComponent;
        [SerializeField] private Button sendButton;
        [SerializeField] private Transform keyboard;
        [SerializeField] private Animator contentAnimator;

        [CanBeNull] public Action OnMessageSentRequest;
        
        private static readonly int KeyboardUp = Animator.StringToHash("Keyboard Up");
        private static readonly int KeyboardDown = Animator.StringToHash("Keyboard Down");

        private void Start()
        {
            foreach (var keyboardKeyComponent in keyboard.GetComponentsInChildren<KeyboardKeyComponent>())
            {
                keyboardKeyComponent.InitializeInputAction(() =>
                {
                    switch (keyboardKeyComponent.GetKeyType())
                    {
                        case KeyboardKeyComponent.KeyType.None: break;
                        case KeyboardKeyComponent.KeyType.Typing: guidedChatInputComponent.OnInput(); break;
                        case KeyboardKeyComponent.KeyType.KeyboardDown: HideKeyboard(); break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                });
            }

            guidedChatInputComponent.OnClick += ShowKeyboard;

            guidedChatInputComponent.OnMessageIntegrity += () =>
            {
                if (!sendButton.gameObject.activeSelf)
                    sendButton.gameObject.SetActive(true);
            };

            guidedChatInputComponent.OnMessageIncomplete += () =>
            {
                if (sendButton.gameObject.activeSelf)
                    sendButton.gameObject.SetActive(false);
            };
            
            sendButton.onClick.ReplaceListeners(OnMessageSendRequest);
        }

        public void SetGuidedChatInputAvailability(bool isAvailable)
        {
            footer.SetActive(isAvailable);
        }

        public void InitializeNewGuidedMessage(string message)
        {
            guidedChatInputComponent.InitializeExpectedMessage(message);
            if (sendButton.gameObject.activeSelf)
                sendButton.gameObject.SetActive(false);
        }

        public void ClearGuidedMessage()
        {
            guidedChatInputComponent.Clear();
        }

        private void OnMessageSendRequest()
        {
            HideKeyboard();
            SetGuidedChatInputAvailability(false);
            OnMessageSentRequest?.Invoke();
        }
        
        private void ShowKeyboard() => contentAnimator.SetTrigger(KeyboardUp);
        private void HideKeyboard() => contentAnimator.SetTrigger(KeyboardDown);
        
    }
}