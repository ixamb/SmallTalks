using System;
using System.Collections.Generic;
using SmallTalks.Data;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.UI.Chat.Components;
using SmallTalks.UI.Chat.Managers;
using TheForge.Extensions;
using TheForge.Services.Views;
using TheForge.Utils;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace SmallTalks.UI.Chat
{
    public sealed class ChatView : View, IChatReceivedHandler
    {
        [Header("View references")]
        [SerializeField] private ChatHeaderComponent chatHeader;
        
        [Header("Chat properties")]
        [SerializeField] private ChatEntryComponent receiverChatEntryComponentPrefab;
        [SerializeField] private ChatEntryComponent senderChatEntryComponentPrefab;
        [SerializeField] private Transform chatContent;
        
        [Header("Guided chat input")]
        [SerializeField] private GuidedChatInputManager guidedChatInputManager;
        
        private IChatExchangeService _chatExchangeService;
        
        private readonly List<GameObject> _messageGroups = new();
        
        // Cached narrative related data
        private Guid _narrativeId;
        private List<NarrativeData.NarrativeEntry> _narrativeEntries = new();

        [Inject]
        private void Construct(IChatExchangeService chatExchangeService)
        {
            _chatExchangeService = chatExchangeService;
        }

        private void Start()
        {
            _chatExchangeService.RegisterChatReceivedHandler(this);
            OnHide += () =>
            {
                guidedChatInputManager.ClearGuidedMessage();
                _narrativeId = Guid.Empty;
            };
        }

        public void Initialize((Sprite, string) senderData, Guid narrativeId, List<NarrativeData.NarrativeEntry> narrativeEntries, int progressStep)
        {
            chatHeader.Initialize(senderData.Item1, senderData.Item2);
            
            NarrativeData.NarrativeEntry.MessageSender? lastSender = null;
            GameObject activeChatGroup = null;
            
            _narrativeId = narrativeId;
            _narrativeEntries = narrativeEntries;
            
            _messageGroups.DestroyAndClear();
            
            for (var i = 0; i < progressStep+1; i++)
            {
                var narrativeEntry = _narrativeEntries[i];
                
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

                AppendMessageToMessageGroup(activeChatGroup, narrativeEntry.Sender, narrativeEntry.Entry);
            }
            
            var available = UpdateGuidedChatInputAvailability(progressStep);
            if (available)
            {
                var message = _narrativeEntries[progressStep + 1].Entry;
                guidedChatInputManager.InitializeNewGuidedMessage(message);
                guidedChatInputManager.OnMessageSentRequest = () => { SendMessage(narrativeId, message); };
            }
        }
        
        public void OnNewChatReceivedHandler(Guid narrativeId, int progressStep)
        {
            if (narrativeId != _narrativeId)
                return;

            if (!IsVisibleAndActive())
                return;

            var messageGroup = GenerateEmptyMessageGroup(NarrativeData.NarrativeEntry.MessageSender.Other);
            AppendMessageToMessageGroup(messageGroup, _narrativeEntries[progressStep].Sender, _narrativeEntries[progressStep].Entry);
            
            var available = UpdateGuidedChatInputAvailability(progressStep);
            if (available)
            {
                var message = _narrativeEntries[progressStep + 1].Entry;
                guidedChatInputManager.InitializeNewGuidedMessage(message);
                guidedChatInputManager.OnMessageSentRequest = () => { SendMessage(narrativeId, message); };
            }
        }

        private void SendMessage(Guid narrativeId, string message)
        {
            _chatExchangeService.SendMessage(narrativeId);
            var messageGroup = GenerateEmptyMessageGroup(NarrativeData.NarrativeEntry.MessageSender.Myself);
            AppendMessageToMessageGroup(messageGroup, NarrativeData.NarrativeEntry.MessageSender.Myself, message);
        }

        private bool UpdateGuidedChatInputAvailability(int progressStep)
        {
            var chatIsAvailable = progressStep + 1 < _narrativeEntries.Count && _narrativeEntries[progressStep + 1].Sender == NarrativeData.NarrativeEntry.MessageSender.Myself;
            guidedChatInputManager.SetGuidedChatInputAvailability(chatIsAvailable);
            return chatIsAvailable;
        }

        public Guid GetActiveNarrativeId() => _narrativeId;
        
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

        private void AppendMessageToMessageGroup(GameObject messageGroup, NarrativeData.NarrativeEntry.MessageSender sender, string message)
        {
            var prefab = sender switch
            {
                NarrativeData.NarrativeEntry.MessageSender.Myself => senderChatEntryComponentPrefab,
                NarrativeData.NarrativeEntry.MessageSender.Other => receiverChatEntryComponentPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(sender), sender, null)
            };
            
            var receiverChatEntryComponent = Instantiate(prefab, messageGroup.transform);
            receiverChatEntryComponent.Initialize(message);
        }

        private GameObject InitializeEmptyReceiverMessageGroup()
        {
            var receiverGroup = new GameObject($"receiverGroup_{StringUtils.Random(8)}");
            var rectTransform = receiverGroup.AddComponent<RectTransform>();
            
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

            receiverGroup.transform.SetParent(chatContent);
            rectTransform.localScale = Vector3.one;

            _messageGroups.Add(receiverGroup);
            return receiverGroup;
        }
        
        private GameObject InitializeEmptySenderMessageGroup()
        {
            var senderGroup = new GameObject($"senderGroup_{StringUtils.Random(8)}");
            var rectTransform = senderGroup.AddComponent<RectTransform>();
            
            var verticalLayoutGroup = senderGroup.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.padding.left = 5;
            verticalLayoutGroup.spacing = 15;
            verticalLayoutGroup.childAlignment = TextAnchor.UpperRight;
            
            verticalLayoutGroup.childControlWidth = false;
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childForceExpandWidth = true;
            verticalLayoutGroup.childForceExpandHeight = true;

            var contentSizeFitter = senderGroup.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

            senderGroup.transform.SetParent(chatContent);
            rectTransform.localScale = Vector3.one;
            
            _messageGroups.Add(senderGroup);
            return senderGroup;
        }
        
        #endregion message groups
    }
}