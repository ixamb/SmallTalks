using System.Collections.Generic;
using SmallTalks.Data;
using UnityEngine;

namespace SmallTalks.Services.GameData
{
    [CreateAssetMenu(fileName = "Game Data Container", menuName = "Small Talks/Services/Game Data Container")]
    public sealed class GameDataContainer : ScriptableObject
    {
        [SerializeField] private List<NarrativeData> narrativeData;
        
        internal List<NarrativeData> NarrativeData => narrativeData;
    }
}