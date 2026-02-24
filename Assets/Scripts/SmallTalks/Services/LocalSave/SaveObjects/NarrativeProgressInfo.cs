using System;

namespace SmallTalks.Services.LocalSave.SaveObjects
{
    public sealed class NarrativeProgressInfo
    {
        public bool Accepted { get; set; }
        public bool IsActive { get; set; }
        public bool HasNewMessage { get; set; }
        public int Progress { get; set; }
        public DateTime LastUpdate { get; set; }

        public NarrativeProgressInfo(bool accepted, bool isActive, int progress)
        {
            Accepted = accepted;
            IsActive = isActive;
            Progress = progress;
            HasNewMessage = false;
            LastUpdate = DateTime.Now;
        }
    }
}