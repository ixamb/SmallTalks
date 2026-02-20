using System;

namespace SmallTalks.Services.LocalSave.SaveObjects
{
    public sealed class NarrativeProgressInfo
    {
        public bool Accepted { get; set; }
        public uint Progress { get; set; }

        public NarrativeProgressInfo(bool accepted, uint progress)
        {
            Accepted = accepted;
            Progress = progress;
        }
    }
}