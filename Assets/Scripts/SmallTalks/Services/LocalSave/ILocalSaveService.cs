using System;
using System.Collections.Generic;
using SmallTalks.Services.LocalSave.SaveObjects;
using TheForge.Services;

namespace SmallTalks.Services.LocalSave
{
    public interface ILocalSaveService : ISingleton
    {
        bool TryGetNarrativeProgressStep(Guid narrativeGuid, out uint? progressStep);
        Dictionary<Guid, NarrativeProgressInfo> GetAllNarrativeProgressSteps();
        void RegisterNarrativeProgress(Guid narrativeId, bool wasAccepted, bool autoSave = true);
        void DeleteNarrativeProgress(Guid narrativeId, bool autoSave = true);
        bool IsNarrativeActive(Guid narrativeGuid);
    }
}