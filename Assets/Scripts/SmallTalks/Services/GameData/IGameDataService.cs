using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SmallTalks.Data;
using TheForge.Services;

namespace SmallTalks.Services.GameData
{
    public interface IGameDataService
    {
        Dictionary<Guid, NarrativeData> GetNarrativeDataDictionary();
        [CanBeNull] NarrativeData GetNarrativeData(Guid narrativeId);
    }
}