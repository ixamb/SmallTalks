using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SmallTalks.UI.Chat.Components
{
    public class GuidedChatInputComponent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text textField;
        
        [CanBeNull] public Action OnClick { get; set; }
        [CanBeNull] public Action OnMessageIntegrity { get; set; }
        
        private char[] _expectedMessage;
        private uint _currentMessageIndex;
        
        public void InitializeExpectedMessage(string expectedMessage)
        {
            _expectedMessage = expectedMessage.ToCharArray();
            _currentMessageIndex = 0;
        }

        public void OnInput()
        {
            if (_currentMessageIndex >= _expectedMessage.Length)
                return;
            textField.text += _expectedMessage[_currentMessageIndex];
            _currentMessageIndex++;

            if (_currentMessageIndex == _expectedMessage.Length)
            {
                OnMessageIntegrity?.Invoke();
            }
        }

        public void OnPointerClick(PointerEventData _)
        {
            OnClick?.Invoke();
        }
    }
}