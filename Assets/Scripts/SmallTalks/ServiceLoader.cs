using UnityEngine;

namespace SmallTalks
{
    public static class ServiceLoader
    {
        private const string Path = "Core/__SERVICESCONTAINER__";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadServiceContainer()
        {
            var serviceContainer = Resources.Load(Path);
            var instance = Object.Instantiate(serviceContainer);
            Object.DontDestroyOnLoad(instance);
        }
    }
}