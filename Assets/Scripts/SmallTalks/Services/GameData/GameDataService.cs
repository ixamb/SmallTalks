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
        
        protected override void Init()
        {
        }

        public Dictionary<Guid, NarrativeData> GetNarrativeDataDictionary()
        {
            return dataContainer.GetNarrativeData().ToDictionary(narrative => narrative.Guid);
        }
        
        public NarrativeData GetNarrativeData(Guid guid)
        {
            return dataContainer.GetNarrativeData().FirstOrDefault(narrative => narrative.Guid == guid);
        }
    }
}