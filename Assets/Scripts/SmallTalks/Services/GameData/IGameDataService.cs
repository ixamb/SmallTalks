using System;
using System.Collections.Generic;
using SmallTalks.Data;
using TheForge.Services;

namespace SmallTalks.Services.GameData
{
    public interface IGameDataService : ISingleton
    {
        Dictionary<Guid, NarrativeData> GetNarrativeData();
    }
}