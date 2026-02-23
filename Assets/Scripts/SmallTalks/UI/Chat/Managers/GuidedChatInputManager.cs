using SmallTalks.UI.Chat.Components;
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

        private static readonly int Property = Animator.StringToHash("Keyboard Up");
        private static readonly int Property1 = Animator.StringToHash("Keyboard Down");

        private void Start()
        {
            foreach (var keyboardKeyComponent in keyboard.GetComponentsInChildren<KeyboardKeyComponent>())
            {
                keyboardKeyComponent.InitializeInputAction(() =>
                {
                    guidedChatInputComponent.OnInput();
                });
            }

            guidedChatInputComponent.OnClick += () =>
            {
                contentAnimator.SetTrigger(Property);
            };

            guidedChatInputComponent.OnMessageIntegrity += () =>
            {
                sendButton.gameObject.SetActive(true);
            };
        }

        public void InitializeNewGuidedMessage(string message)
        {
            guidedChatInputComponent.InitializeExpectedMessage(message);
        }

        private void OnMessageSendRequest()
        {
            contentAnimator.SetTrigger(Property1);
        }
    }
}