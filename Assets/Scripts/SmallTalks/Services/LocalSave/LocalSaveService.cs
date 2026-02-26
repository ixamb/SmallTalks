using System;
using System.Collections.Generic;
using SmallTalks.Services.LocalSave.SaveObjects;
using TheForge.Services;

using ForgeLocalSaveService = TheForge.Services.LocalSave.LocalSaveService;

namespace SmallTalks.Services.LocalSave
{
    public sealed class LocalSaveService : Singleton<LocalSaveService, ILocalSaveService>, ILocalSaveService
    {
        protected override void Init()
        { }

        public Dictionary<Guid, NarrativeProgressInfo> GetAllNarrativeProgressSteps()
        {
            return ForgeLocalSaveService.Instance.Get<Dictionary<Guid, NarrativeProgressInfo>>(Constants.SaveDataEntryKey.NarrativeProgresses)
                   ?? new Dictionary<Guid, NarrativeProgressInfo>();
        }

        public bool TryGetNarrativeProgress(Guid narrativeGuid, out NarrativeProgressInfo progressInfo)
        {
            if (GetAllNarrativeProgressSteps().TryGetValue(narrativeGuid, out var narrativeProgressInfo))
            {
                progressInfo = narrativeProgressInfo;
                return true;
            }
            
            progressInfo = null;
            return false;
        }
        
        public bool TryGetNarrativeProgressStep(Guid narrativeGuid, out int? progressStep)
        {
            if (TryGetNarrativeProgress(narrativeGuid, out var progress))
            {
                progressStep = progress!.Progress;
                return true;
            }

            progressStep = null;
            return false;
        }

        public int GetNarrativeProgressStep(Guid narrativeGuid)
        {
            return TryGetNarrativeProgress(narrativeGuid, out var progress)
                ? progress.Progress
                : throw new Exception($"Narrative with guid {narrativeGuid} not found");
        }

        public void RegisterNewNarrativeProgress(Guid narrativeId, bool wasAccepted, bool autoSave = true)
        {
            var narratives = GetAllNarrativeProgressSteps();
            narratives.TryAdd(narrativeId, new NarrativeProgressInfo(accepted: wasAccepted, isActive: wasAccepted, progress: 0));
            SaveNarrativeProgressOntoForgeService(narratives, autoSave);
        }

        public void DeleteAllNarrativeProgress(bool autoSave = true)
        {
            SaveNarrativeProgressOntoForgeService(new Dictionary<Guid, NarrativeProgressInfo>(), autoSave);
        }

        public void DeleteNarrativeProgress(Guid narrativeId, bool autoSave = true)
        {
            var narratives = GetAllNarrativeProgressSteps();
            narratives.Remove(narrativeId);
            SaveNarrativeProgressOntoForgeService(narratives, autoSave);
        }
        
        public void IncreaseNarrativeProgressStep(Guid narrativeId, int progressIncrease, bool autoSave = true)
        {
            var progresses = GetAllNarrativeProgressSteps();
            if (!progresses.TryGetValue(narrativeId, out var progress))
                return;
            
            progress.Progress += progressIncrease;
            progress.LastUpdate = DateTime.Now;
            SaveNarrativeProgressOntoForgeService(progresses, autoSave);
        }

        public void MarkNarrativeProgressNewMessageStatus(Guid narrativeId, bool hasNewMessage, bool autoSave = true)
        {
            var progresses = GetAllNarrativeProgressSteps();
            if (!progresses.TryGetValue(narrativeId, out var progress))
                return;
            
            progress.HasNewMessage = hasNewMessage;
            SaveNarrativeProgressOntoForgeService(progresses, autoSave);
        }

        #region forge local save functions
        
        public void Save() => ForgeLocalSaveService.Instance.Save();
        
        private void SaveNarrativeProgressOntoForgeService(Dictionary<Guid, NarrativeProgressInfo> narratives, bool autoSave = true)
        {
            ForgeLocalSaveService.Instance.Set(Constants.SaveDataEntryKey.NarrativeProgresses, narratives, autoSave);
        }
        
        #endregion forge local save functions
    }
}