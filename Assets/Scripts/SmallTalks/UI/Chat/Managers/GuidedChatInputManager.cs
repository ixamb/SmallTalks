using SmallTalks.UI.Chat.Components;
using TheForge.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Chat.Managers
{
    public class GuidedChatInputManager : MonoBehaviour
    {
        [SerializeField] private GuidedChatInputComponent guidedChatInputComponent;
        [SerializeField] private Button sendButton;
        [SerializeField] private Transform keyboard;
        [SerializeField] private Animator contentAnimator;

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
                    }
                });
            }

            guidedChatInputComponent.OnClick += ShowKeyboard;

            guidedChatInputComponent.OnMessageIntegrity += () =>
            {
                sendButton.gameObject.SetActive(true);
            };
            
            sendButton.onClick.ReplaceListeners(OnMessageSendRequest);
        }

        public void InitializeNewGuidedMessage(string message)
        {
            guidedChatInputComponent.InitializeExpectedMessage(message);
        }

        private void OnMessageSendRequest()
        {
            HideKeyboard();
        }
        
        private void ShowKeyboard() => contentAnimator.SetTrigger(KeyboardUp);
        private void HideKeyboard() => contentAnimator.SetTrigger(KeyboardDown);
        
    }
}