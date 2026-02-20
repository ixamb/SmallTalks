using SmallTalks.Services.GameData;
using SmallTalks.Services.LocalSave;
using TheForge.Services.Scenes;
using TheForge.Services.Scheduler;
using TheForge.Services.Views;
using UnityEngine;

namespace SmallTalks.Services
{
    [RequireComponent(typeof(IActionSchedulerService))]
    [RequireComponent(typeof(ISceneService))]
    [RequireComponent(typeof(IViewService))]
    [RequireComponent(typeof(TheForge.Services.LocalSave.ILocalSaveService))]
    [RequireComponent(typeof(ILocalSaveService))]
    [RequireComponent(typeof(IGameDataService))]
    public sealed class ServicesContainer : MonoBehaviour
    {
    }
}