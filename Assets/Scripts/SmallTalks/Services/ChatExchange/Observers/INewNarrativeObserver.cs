using System;

namespace SmallTalks.Services.ChatExchange.Observers
{
    public interface INewNarrativeObserver
    {
        void OnNewNarrativeActivated(Guid narrativeId);
    }
}