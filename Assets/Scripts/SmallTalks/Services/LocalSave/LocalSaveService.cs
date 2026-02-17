using System.Collections.Generic;
using SmallTalks.Data;
using TheForge.Services;

namespace SmallTalks.Services.LocalSave
{
    public sealed class LocalSaveService : Singleton<LocalSaveService, ILocalSaveService>, ILocalSaveService
    {
        protected override void Init()
        { }

        public IEnumerable<NarrativeData> GetAvailableNarratives()
        {
            yield break;
        }
    }
}