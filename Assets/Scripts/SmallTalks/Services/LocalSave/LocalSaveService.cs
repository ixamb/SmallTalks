using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SmallTalks.Services.LocalSave.SaveObjects;
using TheForge.Services;

using ForgeLocalSaveService = TheForge.Services.LocalSave.LocalSaveService;

namespace SmallTalks.Services.LocalSave
{
    public sealed class LocalSaveService : Singleton<LocalSaveService, ILocalSaveService>, ILocalSaveService
    {
        protected override void Init()
        { }

        public bool TryGetNarrativeProgressStep(Guid narrativeGuid, out uint? progressStep)
        {
            var narratives = GetAllNarrativeProgressSteps();
            if (narratives.TryGetValue(narrativeGuid, out var narrativeProgressInfo) && narrativeProgressInfo.Accepted)
            {
                progressStep = narrativeProgressInfo.Progress;
                return true;
            }

            progressStep = null;
            return false;
        }
        
        public bool IsNarrativeActive(Guid narrativeGuid)
        {
            var narratives = GetAllNarrativeProgressSteps();
            return narratives?.ContainsKey(narrativeGuid) ?? false;
        }

        public void RegisterNarrativeProgress(Guid narrativeId, bool wasAccepted, bool autoSave = true)
        {
            var narratives = GetAllNarrativeProgressSteps();
            narratives.TryAdd(narrativeId, new NarrativeProgressInfo(wasAccepted, 0));
            SaveNarrativeProgressOntoForgeService(narratives, autoSave);
        }

        public void DeleteNarrativeProgress(Guid narrativeId, bool autoSave = true)
        {
            var narratives = GetAllNarrativeProgressSteps();
            narratives.Remove(narrativeId);
            SaveNarrativeProgressOntoForgeService(narratives, autoSave);
        }
        
        public Dictionary<Guid, NarrativeProgressInfo> GetAllNarrativeProgressSteps()
        {
            return ForgeLocalSaveService.Instance.Get<Dictionary<Guid, NarrativeProgressInfo>>(Constants.SaveDataEntryKey.NarrativeProgresses)
                   ?? new Dictionary<Guid, NarrativeProgressInfo>();
        }

        private void SaveNarrativeProgressOntoForgeService(Dictionary<Guid, NarrativeProgressInfo> narratives, bool autoSave = true)
        {
            ForgeLocalSaveService.Instance.Set(Constants.SaveDataEntryKey.NarrativeProgresses, narratives, autoSave);
        }
    }
}