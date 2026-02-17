using System.Collections.Generic;
using SmallTalks.UI.Chat.Components;
using TheForge.Services.Views;
using TheForge.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Chat
{
    public sealed class ChatView : View
    {
        [SerializeField] private ChatEntryComponent receiverChatEntryComponentPrefab;
        [SerializeField] private ChatEntryComponent senderChatEntryComponentPrefab;
        [SerializeField] private Transform chatContent;
        
        private void Start()
        {
            OnShow += Initialize;
        }
        
        private void Initialize()
        {
        }

        private GameObject GenerateReceiverMessageGroup(IEnumerable<string> messages)
        {
            var receiverGroup = new GameObject($"receiverGroup_{StringUtils.Random(8)}");
            receiverGroup.AddComponent<RectTransform>();
            
            var verticalLayoutGroup = receiverGroup.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.padding.left = 5;
            verticalLayoutGroup.spacing = 15;
            verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            
            verticalLayoutGroup.childControlWidth = false;
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childForceExpandWidth = true;
            verticalLayoutGroup.childForceExpandHeight = true;

            var contentSizeFitter = receiverGroup.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

            foreach (var message in messages)
            {
                var receiverChatEntryComponent = Instantiate(receiverChatEntryComponentPrefab, receiverGroup.transform);
                receiverChatEntryComponent.Initialize(message);
            }
            return receiverGroup;
        }
        
        private GameObject GenerateSenderMessageGroup(IEnumerable<string> messages)
        {
            var receiverGroup = new GameObject($"receiverGroup_{StringUtils.Random(8)}");
            receiverGroup.AddComponent<RectTransform>();
            
            var verticalLayoutGroup = receiverGroup.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.padding.left = 5;
            verticalLayoutGroup.spacing = 15;
            verticalLayoutGroup.childAlignment = TextAnchor.UpperRight;
            
            verticalLayoutGroup.childControlWidth = false;
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childForceExpandWidth = true;
            verticalLayoutGroup.childForceExpandHeight = true;

            var contentSizeFitter = receiverGroup.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

            foreach (var message in messages)
            {
                var receiverChatEntryComponent = Instantiate(senderChatEntryComponentPrefab, receiverGroup.transform);
                receiverChatEntryComponent.Initialize(message);
            }
            return receiverGroup;
        }
    }
}