using TheForge.Services;
using UnityEngine;

namespace SmallTalks.Services.GameData
{
    public sealed class GameDataService : Singleton<GameDataService, IGameDataService>, IGameDataService
    {
        [SerializeField] private GameDataContainer dataContainer;
        
        protected override void Init() { }
        
        
    }
}