using System;
using System.Collections.Generic;
using SmallTalks.Services.LocalSave.SaveObjects;
using TheForge.Services;

namespace SmallTalks.Services.LocalSave
{
    public interface ILocalSaveService : ISingleton
    {
        Dictionary<Guid, NarrativeProgressInfo> GetAllNarrativeProgressSteps();
        bool TryGetNarrativeProgress(Guid narrativeGuid, out NarrativeProgressInfo progressInfo);
        bool TryGetNarrativeProgressStep(Guid narrativeGuid, out int? progressStep);
        void RegisterNewNarrativeProgress(Guid narrativeId, bool wasAccepted, bool autoSave = true);
        void DeleteAllNarrativeProgress(bool autoSave = true);
        void DeleteNarrativeProgress(Guid narrativeId, bool autoSave = true);
        void IncreaseNarrativeProgressStep(Guid narrativeId, int progressIncrease, bool autoSave = true);
        void MarkNarrativeProgressNewMessageStatus(Guid narrativeId, bool hasNewMessage, bool autoSave = true);
        void Save();
    }
}