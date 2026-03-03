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
        
        void RegisterNarrativeRegistrationObserver(INarrativeRegistrationObserver observer);
        void UnregisterNarrativeRegistrationObserver(INarrativeRegistrationObserver observer);
    }
    
    public sealed class NarrativeStackManager : INarrativeStackManager
    {
        private readonly ILocalSaveService _localSaveService;
        
        private readonly Stack<NarrativeData> _pendingNarrativeStack;

        private readonly List<INarrativeRegistrationObserver> _narrativeRegistrationObservers;
        
        public NarrativeStackManager(IGameDataService gameDataService, ILocalSaveService localSaveService)
        {
            _localSaveService = localSaveService;

            var narratives = gameDataService.GetNarrativeData();
            narratives.RemoveAll(narrative => _localSaveService.GetAllNarrativeProgressSteps().ContainsKey(narrative.Guid));
            _pendingNarrativeStack = new Stack<NarrativeData>(narratives);
            _narrativeRegistrationObservers = new List<INarrativeRegistrationObserver>();
        }
        
        public NarrativeData AcceptNarrative()
        {
            var acceptedNarrative = _pendingNarrativeStack.PopOrDefault();
            if (acceptedNarrative is null)
                return null;
            
            _localSaveService.RegisterNewNarrativeProgress(narrativeId: acceptedNarrative.Guid, wasAccepted: true);
            _narrativeRegistrationObservers.ForEach(o => o.OnNewNarrativeAccepted(acceptedNarrative.Guid));
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

        public void RegisterNarrativeRegistrationObserver(INarrativeRegistrationObserver observer)
        {
            _narrativeRegistrationObservers.AddUnique(observer);
        }

        public void UnregisterNarrativeRegistrationObserver(INarrativeRegistrationObserver observer)
        {
            _narrativeRegistrationObservers.RemoveUnique(observer);
        }
    }
    
    public interface INarrativeRegistrationObserver
    {
        void OnNewNarrativeAccepted(Guid narrativeId);
    }
}