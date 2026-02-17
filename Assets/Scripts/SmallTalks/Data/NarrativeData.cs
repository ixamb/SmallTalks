using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTalks.Data
{
    [CreateAssetMenu(fileName = "Narrative Data", menuName = "Small Talks/Data/Narrative Data")]
    public sealed class NarrativeData : ScriptableObject
    {
        [SerializeField] private SenderProfile senderProfile;
        [Space]
        [SerializeField] private List<NarrativeEntry> narrativeEntries;
        
        [Serializable]
        public sealed class NarrativeEntry
        {
            [SerializeField] private MessageSender messageSender;
            [SerializeField] private string entry;
            
            public MessageSender Sender => messageSender;
            public string Entry => entry;
            
            public enum MessageSender
            {
                Myself, Other
            }
        }
        
        [Serializable]
        public sealed class SenderProfile
        {
            [SerializeField] private Sprite profilePicture;
            [SerializeField] private new string name;
            [SerializeField] private string description;
        }
    }
}