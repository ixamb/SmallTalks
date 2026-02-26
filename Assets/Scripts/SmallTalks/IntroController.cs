using TheForge.Services.Delayer;
using TheForge.Services.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SmallTalks
{
    public sealed class IntroController : MonoBehaviour
    {
        [SerializeField] private float introDurationInSeconds = 5f;
        
        private void Awake()
        {
            ActionDelayerService.Instance.Delay(introDurationInSeconds, () => { SceneService.Instance.LoadSceneAsync(Constants.SceneNames.Main, LoadSceneMode.Single); });
        }
    }
}