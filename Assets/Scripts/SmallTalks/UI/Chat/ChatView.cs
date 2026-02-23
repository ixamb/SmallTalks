using System;
using System.Collections.Generic;
using SmallTalks.Data;
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

        public void Initialize(List<NarrativeData.NarrativeEntry> narrativeEntries, uint progress)
        {
            NarrativeData.NarrativeEntry.MessageSender? lastSender = null;
            GameObject activeChatGroup = null;
            for (var i = 0; i < progress; i++)
            {
                var narrativeEntry = narrativeEntries[i];
                
                if (lastSender is null)
                {
                    lastSender = narrativeEntry.Sender;
                    activeChatGroup = GenerateEmptyMessageGroup(lastSender!.Value);
                }
                else
                {
                    if (narrativeEntry.Sender != lastSender.Value)
                    {
                        lastSender = narrativeEntry.Sender;
                        activeChatGroup = GenerateEmptyMessageGroup(lastSender.Value);
                    }
                }

                AppendMessageToMessageGroup(activeChatGroup, narrativeEntry.Entry);
            }
        }
        
        #region message groups

        private GameObject GenerateEmptyMessageGroup(NarrativeData.NarrativeEntry.MessageSender sender)
        {
            return sender switch
            {
                NarrativeData.NarrativeEntry.MessageSender.Myself => InitializeEmptySenderMessageGroup(),
                NarrativeData.NarrativeEntry.MessageSender.Other => InitializeEmptyReceiverMessageGroup(),
                _ => throw new ArgumentOutOfRangeException(nameof(sender), sender, null)
            };
        }

        private void AppendMessageToMessageGroup(GameObject messageGroup, string message)
        {
            var receiverChatEntryComponent = Instantiate(receiverChatEntryComponentPrefab, messageGroup.transform);
            receiverChatEntryComponent.Initialize(message);
        }

        private GameObject InitializeEmptyReceiverMessageGroup()
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

            return receiverGroup;
        }
        
        private GameObject InitializeEmptySenderMessageGroup()
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


            return receiverGroup;
        }
        
        #endregion message groups
    }
}