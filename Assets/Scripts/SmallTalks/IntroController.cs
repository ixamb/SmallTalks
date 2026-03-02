using TheForge.Services.Delayer;
using TheForge.Services.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace SmallTalks
{
    public sealed class IntroController : MonoBehaviour
    {
        [SerializeField] private float introDurationInSeconds = 5f;

        private ISceneService _sceneService;
        private IDelayerService _delayerService;
        
        [Inject]
        private void Construct(ISceneService sceneService, IDelayerService delayerService)
        {
            _sceneService = sceneService;
            _delayerService = delayerService;
        }
        
        private void Awake()
        {
            _delayerService.Delay(introDurationInSeconds, () => { _sceneService.LoadSceneAsync(Constants.SceneNames.Main, LoadSceneMode.Single); });
        }
    }
}