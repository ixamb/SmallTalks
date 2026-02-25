using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace SmallTalks.Data
{
    [CreateAssetMenu(fileName = "Narrative Data", menuName = "Small Talks/Data/Narrative Data")]
    public sealed class NarrativeData : ScriptableObject, ICloneable
    {
        [field: SerializeField]
        private string guid = string.Empty;

        public Guid Guid
        {
            get => string.IsNullOrEmpty(guid) ? Guid.Empty : new Guid(guid);
            private set => guid = value.ToString();
        }
        
        [SerializeField] private SenderProfile senderProfile;
        [Space]
        [SerializeField] private List<Tag> tags;
        [Space]
        [SerializeField] private List<NarrativeEntry> narrativeEntries;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Guid != Guid.Empty)
                return;
            
            Guid = Guid.NewGuid();
            EditorUtility.SetDirty(this);
            EditorApplication.delayCall += DelayedSaveAssets;
        }

        [ContextMenu("Generate new GUID")]
        private void GenerateNewGuidAndSave()
        {
            Guid = Guid.NewGuid();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private void DelayedSaveAssets()
        {
            EditorApplication.delayCall -= DelayedSaveAssets;
            if (this != null)
                AssetDatabase.SaveAssets();
        }
#endif

        public object Clone()
        {
            return Instantiate(this);
        }
        
        public SenderProfile Sender => senderProfile;
        public List<Tag> Tags => tags;
        public List<NarrativeEntry> NarrativeEntries => narrativeEntries;

        [Serializable]
        public sealed class NarrativeEntry : ICloneable
        {
            [SerializeField] private MessageSender messageSender;
            [SerializeField] private string entry;
            
            public MessageSender Sender => messageSender;
            public string Entry => entry;
            
            public enum MessageSender
            {
                Myself, Other
            }
            
            public object Clone()
            {
                return new NarrativeEntry
                {
                    messageSender = messageSender,
                    entry = entry,
                };
            } 
        }
        
        [Serializable]
        public sealed class SenderProfile : ICloneable
        {
            [SerializeField] private Sprite profilePicture;
            [SerializeField] private string name;
            [SerializeField] private string description;
            
            public Sprite ProfilePicture => profilePicture;
            public string Name => name;
            public string Description => description;

            public object Clone()
            {
                return new SenderProfile
                {
                    profilePicture = profilePicture,
                    name = name,
                    description = description,
                };
            }
        }

        [Serializable]
        public sealed class Tag : ICloneable
        {
            [SerializeField] private string text;
            
            public string Text => text;

            public object Clone()
            {
                return new Tag
                {
                    text = text,
                };
            }
        }
    }
}