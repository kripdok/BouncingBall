using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace BouncingBall.Utilities
{
    public class SceneLoader
    {
        public async UniTask LoadSceneAsync(string sceneName, Action onSceneLoadedCallback = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onSceneLoadedCallback?.Invoke();
                return;
            }

            var operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            onSceneLoadedCallback?.Invoke();
        }
    }
}
