using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SmallTalks.Data;
using SmallTalks.Extensions;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;

namespace SmallTalks.UI.Swipe
{
    public interface INarrativeStackManager
    {
        [CanBeNull] NarrativeData AcceptNarrative();
        [CanBeNull] NarrativeData RefuseNarrative();
        [CanBeNull] NarrativeData GetNextNarrative();

        event Action<Guid> OnNewNarrativeAccepted;
    }
    
    public sealed class NarrativeStackManager : INarrativeStackManager
    {
        private readonly ILocalSaveService _localSaveService;
        
        private readonly Stack<NarrativeData> _pendingNarrativeStack;

        public event Action<Guid> OnNewNarrativeAccepted = delegate { };
        
        public NarrativeStackManager(IGameDataService gameDataService, ILocalSaveService localSaveService)
        {
            _localSaveService = localSaveService;

            var narratives = gameDataService.GetNarrativeData();
            narratives.RemoveAll(narrative => _localSaveService.GetAllNarrativeProgressSteps().ContainsKey(narrative.Guid));
            _pendingNarrativeStack = new Stack<NarrativeData>(narratives);
        }
        
        public NarrativeData AcceptNarrative()
        {
            var acceptedNarrative = _pendingNarrativeStack.PopOrDefault();
            if (acceptedNarrative is null)
                return null;
            
            _localSaveService.RegisterNewNarrativeProgress(narrativeId: acceptedNarrative.Guid, wasAccepted: true);
            OnNewNarrativeAccepted.Invoke(acceptedNarrative.Guid);
            return acceptedNarrative;
        }

        public NarrativeData RefuseNarrative()
        {
            var refusedNarrative = _pendingNarrativeStack.PopOrDefault();
            if (refusedNarrative is null)
                return null;
            
            _localSaveService.RegisterNewNarrativeProgress(narrativeId: refusedNarrative.Guid, wasAccepted: false);
            return refusedNarrative;
        }
        
        public NarrativeData GetNextNarrative()
        {
            return _pendingNarrativeStack.PeekOrDefault();
        }
    }
}