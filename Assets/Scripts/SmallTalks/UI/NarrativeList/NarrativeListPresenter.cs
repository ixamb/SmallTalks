using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using SmallTalks.Data;
using SmallTalks.Services.ChatExchange;
using SmallTalks.Services.ChatExchange.Observers;
using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;

namespace SmallTalks.UI.NarrativeList
{
    public interface INarrativePreviewListPresenter
    {
        event Action<NarrativePreviewListPresenter.ActiveNarrativeEntryPreview> OnNewNarrativeAdded;
        event Action<NarrativePreviewListPresenter.ActiveNarrativeEntryPreview, string> OnChatMessageReceived;
        event Action<Guid> OnChatMessageRead;
        
        List<NarrativePreviewListPresenter.ActiveNarrativeEntryPreview> GetActiveNarrativePreviews();
        void MarkMessageAsRead(Guid narrativeId);
    }
    
    public sealed class NarrativePreviewListPresenter : INarrativePreviewListPresenter, INewNarrativeObserver, INewChatMessageObserver
    {
        private readonly IGameDataService _gameDataService;
        private readonly ILocalSaveService _localSaveService;
        
        private readonly OrderedDictionary _activeNarrativeData = new();
        
        public event Action<ActiveNarrativeEntryPreview> OnNewNarrativeAdded;
        public event Action<ActiveNarrativeEntryPreview, string> OnChatMessageReceived;
        public event Action<Guid> OnChatMessageRead;
        
        public NarrativePreviewListPresenter(IGameDataService gameDataService, ILocalSaveService localSaveService, IChatExchangeService chatExchangeService)
        {
            _gameDataService = gameDataService;
            _localSaveService = localSaveService;
            
            chatExchangeService.RegisterNewChatMessageObserver(this);
            chatExchangeService.RegisterNewNarrativeObserver(this);
            
            var narrativeData = _gameDataService.GetNarrativeDataDictionary();

            foreach (var runningNarrativeKvp in _localSaveService.GetAllNarrativeProgressSteps()
                         .Where(runningNarrativeKvp => runningNarrativeKvp.Value.Accepted)
                         .OrderBy(runningNarrativeKvp => runningNarrativeKvp.Value.LastUpdate))
            {
                if (!narrativeData.TryGetValue(runningNarrativeKvp.Key, out var data))
                    continue;

                _activeNarrativeData.Add(data.Guid,
                    ActiveNarrativeEntryPreview.FromData(data, runningNarrativeKvp.Value.Progress, runningNarrativeKvp.Value.HasNewMessage));
            }
        }
        
        public List<ActiveNarrativeEntryPreview> GetActiveNarrativePreviews()
        {
            return _activeNarrativeData.Values.Cast<ActiveNarrativeEntryPreview>().ToList();
        }
        
        // observer function
        public void OnNewNarrativeActivated(Guid narrativeId)
        {
            if (_activeNarrativeData.Contains(narrativeId))
                return;
            
            var newEntryPreview = ActiveNarrativeEntryPreview.FromData(_gameDataService.GetNarrativeData(narrativeId), 0, true);
            newEntryPreview.HasNewMessage = true;
            _activeNarrativeData.Insert(0, narrativeId, newEntryPreview);
            OnNewNarrativeAdded?.Invoke(newEntryPreview);
        }

        public void MarkMessageAsRead(Guid narrativeId)
        {
            ((ActiveNarrativeEntryPreview) _activeNarrativeData[narrativeId]).HasNewMessage = false;
            _localSaveService.MarkNarrativeProgressNewMessageStatus(narrativeId, false);
            OnChatMessageRead?.Invoke(narrativeId);
        }
        
        // observer function
        public void OnNewChatMessageReceived(Guid narrativeId, int progressStep)
        {
            if (!_activeNarrativeData.Contains(narrativeId))
                return;
            
            var entry = (ActiveNarrativeEntryPreview)_activeNarrativeData[narrativeId];
            entry.ProgressStep = progressStep;
            entry.HasNewMessage = true;
            OnChatMessageReceived?.Invoke(entry, entry.Data.NarrativeEntries[progressStep].Entry);
        }

        public sealed class ActiveNarrativeEntryPreview
        {
            public int ProgressStep { get; internal set; }
            public bool HasNewMessage { get; internal set; }
            public NarrativeData Data { get; private set; }
            
            public static ActiveNarrativeEntryPreview FromData(NarrativeData narrativeData, int progressStep, bool hasNewMessage)
            {
                return new ActiveNarrativeEntryPreview
                {
                    ProgressStep = progressStep,
                    HasNewMessage = hasNewMessage,
                    Data = narrativeData,
                };
            }
            
            public string LastMessage() => Data.NarrativeEntries[ProgressStep]?.Entry ?? string.Empty;
        }
    }
}