using TheForge.Services.Scenes;
using TheForge.Services.Scheduler;
using TheForge.Services.Views;
using UnityEngine;

namespace SmallTalks.Services
{
    [RequireComponent(typeof(IActionSchedulerService))]
    [RequireComponent(typeof(ISceneService))]
    [RequireComponent(typeof(IViewService))]
    public sealed class ServicesContainer : MonoBehaviour
    {
    }
}