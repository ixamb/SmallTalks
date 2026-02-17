using System.Collections.Generic;
using SmallTalks.Data;
using TheForge.Services;

namespace SmallTalks.Services.LocalSave
{
    public interface ILocalSaveService : ISingleton
    {
        IEnumerable<NarrativeData> GetAvailableNarratives();
    }
}