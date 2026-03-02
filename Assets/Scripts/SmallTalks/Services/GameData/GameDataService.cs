using System;
using System.Collections.Generic;
using System.Linq;
using SmallTalks.Data;

namespace SmallTalks.Services.GameData
{
    public sealed class GameDataService : IGameDataService
    { 
        private readonly GameDataContainer _dataContainer;

        public GameDataService(GameDataContainer dataContainer)
        {
            _dataContainer = dataContainer;
        }
        
        public Dictionary<Guid, NarrativeData> GetNarrativeDataDictionary()
        {
            return _dataContainer.GetNarrativeData().ToDictionary(narrative => narrative.Guid);
        }
        
        public NarrativeData GetNarrativeData(Guid guid)
        {
            return _dataContainer.GetNarrativeData().FirstOrDefault(narrative => narrative.Guid == guid);
        }
    }
}