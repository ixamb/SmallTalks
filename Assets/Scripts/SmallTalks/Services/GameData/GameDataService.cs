using System;
using System.Collections.Generic;
using System.Linq;
using SmallTalks.Data;
using TheForge.Services;
using UnityEngine;

namespace SmallTalks.Services.GameData
{
    public sealed class GameDataService : Singleton<GameDataService, IGameDataService>, IGameDataService
    {
        [SerializeField] private GameDataContainer dataContainer;

        private Dictionary<Guid, NarrativeData> _narrativeData = new();
        
        protected override void Init()
        {
            _narrativeData = dataContainer.NarrativeData.ToDictionary(data => data.Guid, data => data);
        }
        
        public Dictionary<Guid, NarrativeData> NarrativeData() => _narrativeData;
    }
}